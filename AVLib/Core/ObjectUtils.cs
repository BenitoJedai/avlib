using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using AVLib.Draw.DrawRects;

namespace AVLib.Utils
{
    public class PropertyRef
    {
        public object obj;
        public string propertyPath;
        public object Value
        {
            get { return obj.GetProperty(propertyPath); }
            set { obj.SetProperty(propertyPath, value); }
        }
        public object this[object obj]
        {
            get { return obj.GetProperty(propertyPath); }
            set { obj.SetProperty(propertyPath, value); }
        }
    }

    public static class ObjectUtils
    {
        public static object GetProperty(this object target, string Path)
        {
            if (target is DrawRect && ((DrawRect)target).Value.ContainsProperty(Path))
                return ((DrawRect) target).Value[Path];

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
            if (target is DrawRect && ((DrawRect)target).Value.ContainsProperty(Path))
            {
                ((DrawRect) target).Value[Path] = value;
                return;
            }

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

        public static PropertyRef PropertyRef(this object obj, string propertyPath)
        {
            return new PropertyRef(){obj = obj, propertyPath = propertyPath};
        }
    }

    public static class ObjectConvertUtils
    {
        public static string AsString(this object obj)
        {
            if (obj == null) return string.Empty;
            return obj.ToString();
        }

        public static int AsInteger(this object obj)
        {
            if (obj == null) return 0;
            if (obj is int) return (int) obj;
            if (obj is byte) return (byte) obj;
            int n;
            if (obj is string && int.TryParse((string)obj, out n)) return n;
            if (obj is double) return (int) (double) obj;
            if (obj is float) return (int) (float) obj;
            return 0;
        }

        public static Color AsColor(this object obj)
        {
            if (obj == null) return Color.Black;
            if (obj is Color) return (Color) obj;
            return Color.FromArgb(obj.AsInteger());
        }

        public static bool AsBoolean(this object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool) obj;
            return false;
        }

        public static T As<T>(this object obj)
        {
            return (T) obj;
        }
    }
}
