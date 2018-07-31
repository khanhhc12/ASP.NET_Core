using System;

namespace HelloEPPlus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SimpleEPPlus.ReadExcel();
            SimpleEPPlus.ReadExcelManual();
            SimpleEPPlus.WriteExcel();
        }
    }
}
