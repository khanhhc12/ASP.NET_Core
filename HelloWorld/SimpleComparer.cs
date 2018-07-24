using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace HelloWorld
{
    public static class SimpleComparer
    {
        public static void Intersect()
        {
            var list1 = new List<SimpleModel>
            {
                new SimpleModel { id=1, name ="A" },
                new SimpleModel { id=2, name ="B" },
                new SimpleModel { id=3, name ="C" },
            };
            var list2 = new List<SimpleModel>
            {
                new SimpleModel { id=1, name ="A" },
                new SimpleModel { id=3, name ="C" },
                new SimpleModel { id=4, name ="D" },
            };
            var list = list1.Intersect(list2, new SimpleModelComparer());
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }

    public class SimpleModelComparer : IEqualityComparer<SimpleModel>
    {
        // equal if their Codes are equal
        public bool Equals(SimpleModel x, SimpleModel y)
        {
            // reference the same objects?
            if (Object.ReferenceEquals(x, y)) return true;

            // is either null?
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.id == y.id && x.name == y.name;
        }

        public int GetHashCode(SimpleModel x)
        {
            // If Equals() returns true for a pair of objects 
            // then GetHashCode() must return the same value for these objects.

            // if null default to 0
            if (Object.ReferenceEquals(x, null)) return 0;

            return x.id.GetHashCode();
        }
    }

    public class SimpleModel
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
