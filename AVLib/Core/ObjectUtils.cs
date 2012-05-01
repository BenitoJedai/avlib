using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AVLib.Utils
{
    public static class ObjectUtils
    {
        public static object GetProperty(this object target, string Path)
        {
            PropertyInfo pInfo;
            FieldInfo fieldInfo;
            MethodInfo methodInfo;
            string errorProperty;
            object propOfObject = target;
            if (FindPath(target, Path, out pInfo, out fieldInfo, out methodInfo, ref propOfObject, out errorProperty))
            {
                if (pInfo != null)
                    return pInfo.GetValue(propOfObject, null);
                if (fieldInfo != null)
                    return fieldInfo.GetValue(propOfObject);
                if (methodInfo != null)
                    return methodInfo.Invoke(propOfObject, null);
            }
            return null;
        }

        public static void SetProperty(this object target, string Path, object value)
        {
            PropertyInfo pInfo;
            FieldInfo fieldInfo;
            MethodInfo methodInfo;
            string errorProperty;
            object propOfObject = target;
            if (FindPath(target, Path, out pInfo, out fieldInfo, out methodInfo, ref propOfObject, out errorProperty))
            {
                if (pInfo != null)
                    pInfo.SetValue(propOfObject, value, null);
                else
                    fieldInfo.SetValue(propOfObject, value);
                return;
            }
        }

        private static bool FindPath(object target, string Path, out PropertyInfo pInfo, out FieldInfo fieldInfo, out MethodInfo methodInfo, ref object propOfObect, out string errorProperty)
        {
            pInfo = null;
            fieldInfo = null;
            methodInfo = null;

            var n = Path.IndexOf('.');
            if (n < 0)
            {
                errorProperty = Path;
                pInfo = target.GetType().GetProperty(Path);
                if (pInfo == null)
                {
                    fieldInfo = target.GetType().GetField(Path);
                    if (fieldInfo == null) methodInfo = target.GetType().GetMethod(Path);
                }
            }
            else
            {
                var s = Path.Substring(0, n);
                Path = Path.Substring(n + 1);
                errorProperty = s;
                var nextProp = target.GetType().GetProperty(s);
                if (nextProp != null)
                {
                    propOfObect = nextProp.GetValue(target, null);
                    if (propOfObect != null)
                    {
                        return FindPath(propOfObect, Path, out pInfo, out fieldInfo, out methodInfo, ref propOfObect, out errorProperty);
                    }
                }
                else
                {
                    var nextField = target.GetType().GetField(s);
                    if (nextField != null)
                    {
                        propOfObect = nextField.GetValue(target);
                        if (propOfObect != null)
                        {
                            return FindPath(propOfObect, Path, out pInfo, out fieldInfo, out methodInfo, ref propOfObect,
                                            out errorProperty);
                        }
                    }
                }
            }

            return (pInfo != null) || (fieldInfo != null) || (methodInfo != null);
        }
    }
}
