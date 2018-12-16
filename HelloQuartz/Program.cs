using System;
using System.IO;
using System.Reflection;

namespace HelloQuartz
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirJson = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Json");
            FileWatcher fileWatcher = new FileWatcher();
            fileWatcher.CreateFileWatcher(dirJson);
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
