using System;
using System.Collections.Generic;

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