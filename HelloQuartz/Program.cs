using System;

namespace HelloQuartz
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleQuartz simpleQuartz = new SimpleQuartz();
            simpleQuartz.Run();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
