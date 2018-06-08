using System;

namespace HelloDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleDapper.Insert();
            SimpleDapper.Delete();
            SimpleDapper.Select();
            Console.WriteLine("Hello World!");
        }
    }
}
