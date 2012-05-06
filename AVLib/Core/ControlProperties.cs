using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AVLib.Utils;

namespace VALib.Draw.Controls
{
    public class ControlProperty
    {
        private object m_value;
        public Func<object, object> Modifier;
        public event Action Changed;
        public event Action RefChanged;
        public event Action AnyChanged;

        private object Modify(object obj)
        {
            return (Modifier == null || obj == null) ? obj : Modifier(obj);
        }
        
        public ControlProperty() {}

        public ControlProperty(object value) { m_value = value; }

        public static object GetValue(object obj)
        {
            if (obj != null)
            {
                if (obj is ControlProperty)
                    return ((ControlProperty)obj).Value;
                if (obj is Func<object>)
                    return ((Func<object>)obj)();
                if (obj is PropertyRef)
                    return ((PropertyRef)obj).Value;
            }
            return obj;
        }

        public object Value
        {
            get { return Modify(GetValue(m_value)); }
            set { SetValue(value); }
        }

        public bool SetValue(object newValue)
        {
            if (!Equals(m_value, newValue))
            {
                if (m_value != null && m_value is ControlProperty)
                {
                    if (!(newValue is ControlProperty) && !(newValue is Func<object>) && !(newValue is PropertyRef))
                    {
                        return ((ControlProperty)m_value).SetValue(newValue);
                    }
                    ((ControlProperty)m_value).Changed -= DoRefChanged;
                    ((ControlProperty)m_value).RefChanged -= DoRefChanged;
                }
                m_value = newValue;
                if (m_value != null && m_value is ControlProperty)
                {
                    ((ControlProperty)m_value).Changed += DoRefChanged;
                    ((ControlProperty)m_value).RefChanged += DoRefChanged;
                }
                DoChanged();
                return true;
            }
            return false;
        }

        public void Clear()
        {
            SetValue(null);
        }

        private void DoChanged()
        {
            if (Changed != null) Changed();
            if (AnyChanged != null) AnyChanged();
        }

        private void DoRefChanged()
        {
            if (RefChanged != null) RefChanged();
            if (AnyChanged != null) AnyChanged();
        }

        internal void Removed()
        {
            if (m_value != null && m_value is ControlProperty) ((ControlProperty)m_value).Changed -= DoChanged;
        }
    }


    public interface IControlProperties
    {
        ControlProperty this[string property, object initValue] { get; }
        ControlProperty this[string property] { get; }
        void Remove(string property);
        bool ContainsProperty(string property);
    }

    public interface IControlPropertiesValue
    {
        object this[string property] { get; set; }
        object this[string property, object defValue] { get; }
        object this[string property, Func<object> defValue] { get; }
        object this[string property, Func<object, object> modifier] { set; }
        bool ContainsProperty(string property);
    }

    public class ControlProperties : IControlPropertiesValue, IControlProperties
    {
        private Dictionary<string, ControlProperty> m_properties = new Dictionary<string, ControlProperty>();

        private bool SetProperty(string property, object value)
        {
            if (m_properties.ContainsKey(property))
            {
                return m_properties[property].SetValue(value);
            }
            m_properties.Add(property, new ControlProperty(value));
            return true;
        }

        private bool SetProperty(string property, object value, Func<object, object> modifier)
        {
            ControlProperty prop;
            if (m_properties.TryGetValue(property, out prop))
            {
                prop.Modifier = modifier;
                return prop.SetValue(value);
            }
            m_properties.Add(property, new ControlProperty(value){Modifier = modifier});
            return true;
        }

        ControlProperty IControlProperties.this[string property]
        {
            get { return ((IControlProperties)this)[property, (object)null]; }
        }

        ControlProperty IControlProperties.this[string property, object initValue]
        {
            get
            {
                if (m_properties.ContainsKey(property))
                {
                    return m_properties[property];
                }
                var res = new ControlProperty(initValue);
                m_properties.Add(property, res);
                return res;
            }
        }

        public bool ContainsProperty(string property)
        {
            return m_properties.ContainsKey(property);
        }

        public object this[string property]
        {
            get
            {
                ControlProperty res;
                if (m_properties.TryGetValue(property, out res))
                    return res.Value;
                return null;
            }
            set { SetProperty(property, value); }
        }

        public object this[string property, Func<object, object> modifier]
        {
            set { SetProperty(property, value, modifier); }
        }

        public object this[string property, object defValue]
        {
            get { return this[property] ?? ControlProperty.GetValue(defValue); }
        }

        public object this[string property, Func<object> defValue]
        {
            get { return this[property] ?? ControlProperty.GetValue(defValue()); }
        }

        public void Remove(string property)
        {
            ControlProperty item;
            if (m_properties.TryGetValue(property, out item))
            {
                item.Removed();
                m_properties.Remove(property);
            }
        }
    }
}
