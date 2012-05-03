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
        public event Action Changed;

        
        public ControlProperty()
        {
            
        }

        public ControlProperty(object value)
        {
            m_value = value;
        }

        public object Value
        {
            get
            {
                if (m_value != null)
                {
                    if (m_value is ControlProperty)
                        return ((ControlProperty) m_value).Value;
                    if (m_value is PropertyRef)
                        return ((PropertyRef) m_value).Value;
                }
                return m_value;
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

        private void DoChanged()
        {
            if (Changed != null) Changed();
        }

        public object ValueOr(object defaultValue)
        {
            return Value ?? defaultValue;
        }

        public T ValueAs<T>()
        {
            return (T)Value;
        }

        public T ValueAs<T>(object defaultValue)
        {
            object v = Value;
            return v == null ? (T) defaultValue : (T) v;
        }

        internal void Removed()
        {
            if (m_value != null && m_value is ControlProperty) ((ControlProperty)m_value).Changed -= DoChanged;
        }
    }

    public class ControlProperties
    {
        private Dictionary<string, ControlProperty> m_properties = new Dictionary<string, ControlProperty>();

        public bool SetProperty(string property, object value)
        {
            if (m_properties.ContainsKey(property))
            {
                return m_properties[property].SetValue(value);
            }
            m_properties.Add(property, new ControlProperty(value));
            return true;
        }

        public ControlProperty Get(string property)
        {
            return Get(property, null);
        }

        public ControlProperty Get(string property, object initValue)
        {
            if (m_properties.ContainsKey(property))
            {
                return m_properties[property];
            }
            var res = new ControlProperty(initValue);
            m_properties.Add(property, res);
            return res;
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

        public object this[string property, object defValue]
        {
            get
            {
                var res = this[property];
                if (res != null) return res;
                return defValue;
            }
        }

        public T GetAs<T>(string property)
        {
            return GetAs<T>(property, null);
        }

        public T GetAs<T>(string property, object defValue)
        {
            var obj = this[property, defValue];
            return obj != null ? (T) obj : (T) defValue;
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
