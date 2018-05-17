using System;
using System.Threading;
using System.Threading.Tasks;

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
    }
}