using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HelloWorld
{
    public class SimpleTask
    {
        public static void Test()
        {
            var task = Task.Run(() => SomeMethod(500));
            Console.Write("500: ");
            if (task.Wait(1000))
                Console.WriteLine(task.Result);
            else
                Console.WriteLine("Timed out");

            task = Task.Run(() => SomeMethod(2000));
            Console.Write("2000: ");
            if (task.Wait(1000))
                Console.WriteLine(task.Result);
            else
                Console.WriteLine("Timed out");
        }

        public static int SomeMethod(int input)
        {
            Thread.Sleep(input);
            return input;
        }

        public static void Test2()
        {
            var list = new List<int>();
            for (int i = 1; i <= 1000; i++)
                list.Add(i);
            var sList = SimpleConvert.SplitList<int>(list, 50);
            var nList = sList.AsParallel().SelectMany(l => l.Select(i => i.ToString())).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(nList));
        }
    }
}
