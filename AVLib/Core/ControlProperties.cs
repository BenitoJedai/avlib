using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AVLib.Utils;

namespace VALib.Draw.Controls
{
    public class ControlPropertyValue
    {
        public Func<bool> Condition;
        public Func<object, object> Modifier; 
        private object m_value;

        private object Modify(object obj)
        {
            return (Modifier == null || obj == null) ? obj : Modifier(obj);
        }

        public object Value
        {
            get
            {
                if (m_value != null)
                {
                    if (m_value is ControlProperty)
                        return Modify(((ControlProperty)m_value).Value);
                    if (m_value is Func<object>)
                        return Modify(((Func<object>)m_value)());
                    if (m_value is PropertyRef)
                        return Modify(((PropertyRef)m_value).Value);
                    if (m_value is ControlPropertyValues)
                        return Modify(((ControlPropertyValues)m_value).Value);
                }
                return Modify(m_value);
            }
            set { m_value = value; }
        }
    }

    internal class ControlPropertyValues
    {
        List<ControlPropertyValue> m_values = new List<ControlPropertyValue>(); 

        public void Clear()
        {
            m_values.Clear();
        }

        public ControlPropertyValue Add(Func<bool> condition, object value, Func<object, object> modifier)
        {
            var res = new ControlPropertyValue() {Condition = condition, Value = value};
            m_values.Add(res);
            return res;
        }

        private ControlPropertyValue NullCondition
        {
            get
            {
                foreach (var val in m_values)
                {
                    if (val.Condition == null) return val;
                }
                return Add(null, null, null);
            }
        }

        public object Value
        {
            get
            {
                foreach (var val in m_values)
                {
                    if (val.Condition == null || val.Condition()) return val.Value;
                }
                return null;
            }
            set { NullCondition.Value = value; }
        }
    }

    public class ControlProperty
    {
        private object m_value;
        public Func<object, object> Modifier;
        public event Action Changed;

        private object Modify(object obj)
        {
            return (Modifier == null || obj == null) ? obj : Modifier(obj);
        }
        
        public ControlProperty() {}

        public ControlProperty(object value) { m_value = value; }

        public object Value
        {
            get
            {
                if (m_value != null)
                {
                    if (m_value is ControlProperty)
                        return Modify(((ControlProperty)m_value).Value);
                    if (m_value is Func<object>)
                        return Modify(((Func<object>)m_value)());
                    if (m_value is PropertyRef)
                        return Modify(((PropertyRef)m_value).Value);
                    if (m_value is ControlPropertyValues)
                        return Modify(((ControlPropertyValues)m_value).Value);
                }
                return Modify(m_value);
            }
            set { SetValue(value); }
        }

        public bool SetValue(object newValue)
        {
            if (m_value != newValue)
            {
                if (m_value != null && m_value is ControlProperty) ((ControlProperty)m_value).Changed -= DoChanged;
                m_value = newValue;
                if (m_value != null && m_value is ControlProperty) ((ControlProperty)m_value).Changed += DoChanged;
                DoChanged();
                return true;
            }
            return false;
        }

        public void Clear()
        {
            SetValue(null);
        }

        private ControlPropertyValues AsControlPropertyValues
        {
            get
            {
                if (m_value == null || !(m_value is ControlPropertyValues))
                    Value = new ControlPropertyValues();
                return (ControlPropertyValues)m_value;
            }
        }

        public ControlPropertyValue this[object value]
        {
            get { return AsControlPropertyValues.Add(null, value, null); }
        }

        public ControlPropertyValue this[object value, Func<object, object> modifier]
        {
            get { return AsControlPropertyValues.Add(null, value, modifier); }
        }

        public ControlPropertyValue this[Func<bool> condition]
        {
            get { return AsControlPropertyValues.Add(condition, null, null); }
        }

        private void DoChanged()
        {
            if (Changed != null) Changed();
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
    }

    public interface IControlPropertiesValue
    {
        object this[string property] { get; set; }
        object this[string property, object defValue] { get; }
        object this[string property, Func<object, object> modifier] { set; }
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
            get { return this[property] ?? defValue; }
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
