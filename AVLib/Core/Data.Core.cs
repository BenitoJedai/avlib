using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AVLib.Data
{
    public class ObjectProperty
    {
        private bool m_canGet;
        private bool m_canSet;
        private string m_Path;
        private bool m_initialized;
        private ObjectProperty m_child = null;

        public ObjectProperty()
        {
            
        }

        public ObjectProperty(string Path)
        {
            m_Path = Path;
        }

        #region Getter/Setter

        private Func<object, object> fGet;
        private Action<object, object> fSet;

        private PropertyInfo m_propInfo;
        private FieldInfo m_fieldInfo;
        private MethodInfo m_methodInfo;

        private object GetProperty(object target)
        {
            return m_propInfo.GetValue(target, null);
        }
        private void SetProperty(object target, object value)
        {
            m_propInfo.SetValue(target, value, null);
        }
        private object GetField(object target)
        {
            return m_fieldInfo.GetValue(target);
        }
        private void SetField(object target, object value)
        {
            m_fieldInfo.SetValue(target, value);
        }
        private object GetMethod(object target)
        {
            return m_methodInfo.Invoke(target, null);
        }

        #endregion

        public bool CanGet
        {
            get { return m_canGet; }
        }

        public bool CanSet
        {
            get { return m_canSet; }
        }

        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; }
        }

        public bool Initialized
        {
            get { return m_initialized; }
        }

        public bool InitProperty(Type targetType)
        {
            return InitProperty(targetType, m_Path);
        }

        public bool InitProperty(Type targetType, string propertyPath)
        {
            string prop, subpath = "";
            var n = Path.IndexOf('.');
            if (n < 0)
            {
                prop = Path;
            }
            else
            {
                prop = Path.Substring(0, n);
                subpath = Path.Substring(n + 1);
            }

            Type propType = null;
            m_propInfo = targetType.GetProperty(prop);
            if (m_propInfo != null)
            {
                fGet = m_propInfo.CanRead ? GetProperty : (Func<object, object>)null;
                fSet = m_propInfo.CanWrite ? SetProperty : (Action<object, object>)null;
                propType = m_propInfo.PropertyType;
            }
            else
            {
                m_fieldInfo = targetType.GetField(prop);
                if (m_fieldInfo != null)
                {
                    fGet = GetField;
                    fSet = SetField;
                    propType = m_fieldInfo.FieldType;
                }
                else
                {
                    m_methodInfo = targetType.GetMethod(prop);
                    if (m_methodInfo != null)
                    {
                        fGet = GetMethod;
                        fSet = null;
                        propType = m_methodInfo.ReturnType;
                    }
                }
            }

            if (propType == null) return false;
            
            if (subpath != "")
            {
                m_child = new ObjectProperty();
                m_initialized = m_child.InitProperty(propType, subpath);
            }
            else
            {
                m_initialized = true;
            }

            m_canGet = (fGet != null) && (m_child == null || m_child.CanGet);
            m_canSet = (fSet != null && m_child == null) || (m_child != null && m_child.CanSet);
            m_Path = propertyPath;

            return m_initialized;
        }

        public object Get(object target)
        {
            if (m_initialized && m_canGet && target != null)
            {
                var o = fGet(target);
                if (m_child != null)
                    return m_child.Get(o);
                return o;
            }
            return null;
        }

        public void Set(object target, object value)
        {
            if (m_initialized && m_canSet && target != null)
            {
                if (m_child == null)
                {
                    fSet(target, value);
                }
                else
                {
                    var o = fGet(target);
                    if (o != null) m_child.Set(o, value);
                }
            }
        }

        public object this[object target]
        {
            get { return Get(target); }
            set { Set(target, value); }
        }
    }

}
