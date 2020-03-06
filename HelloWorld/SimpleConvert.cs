using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloWorld
{
    public static class SimpleConvert
    {
        public static T To<T>(this object value)
        {
            if (value == null)
                return default(T);

            Type t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type valueType = t.GetGenericArguments()[0];
                if (value == null)
                    return default(T);
                object result = Convert.ChangeType(value, valueType);
                return (T)result;
            }
            else if (typeof(T).IsEnum)
            {
                object result = Enum.Parse(typeof(T), value.ToString());
                return (T)result;
            }
            else
            {
                try
                {
                    object result = Convert.ChangeType(value, typeof(T));
                    return (T)result;
                }
                catch
                {
                    //Debug.WriteLine(err.Message);
                    return default(T);
                }
            }
        }

        private static T DictionaryToObject<T>(IDictionary<string, object> dict) where T : new()
        {
            var t = new T();
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, object> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? etc) the CURRENT property is...
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;

                // Fix nullables...
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // ...and change the type
                if (item.Value != null)
                {
                    object newA = Convert.ChangeType(item.Value, newT);
                    t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
                }
            }
            return t;
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> list, int nSize)
        {
            //default value
            nSize = nSize > 0 ? nSize : 30;
            for (int i = 0; i < list.Count; i += nSize)
            {
                yield return list.GetRange(i, Math.Min(nSize, list.Count - i));
            }
        }
    }
}
