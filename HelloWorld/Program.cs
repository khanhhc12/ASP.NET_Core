using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task timeout
            SimpleTask.Test();
            var i = SimpleConvert.To<int>("123");
            Console.WriteLine("Hello World!");
            Console.Read();
        }


    }
}
