using System;
using System.IO;
using System.Reflection;

namespace HelloAssembly
{
    public static class SimpleAssembly
    {
        public static void Run()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            foreach (var item in configuration.GetSection("DLLs").GetChildren())
            {
                // Step 1: load assembly to memory
                var assembly = Assembly.LoadFrom(Directory.GetCurrentDirectory() + "\\DLLs\\" + item["assemblyFile"]);

                // Step 2: get type from the assembly by name
                var type = assembly.GetType(item["class"]);

                // Step 3: get method of the type
                var method = type.GetMethod(item["method"]);

                if (type.IsAbstract)
                {
                    // Step 4: invoke the instance method
                    var result = method.Invoke(null, null);
                    Console.WriteLine(result);
                }
                else
                {
                    // Step 4: create instance of the type
                    var funcs = Activator.CreateInstance(type);

                    // Step 5: invoke the instance method
                    var result = method.Invoke(funcs, null);
                    Console.WriteLine(result);
                }

            }
        }
    }
}