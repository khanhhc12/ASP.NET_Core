using System.IO;
using Microsoft.Extensions.Configuration;

namespace HelloAssembly
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfiguration(string path, string environmentName = null, bool addUserSecrets = false)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        
            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                builder = builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
            }
        
            builder = builder.AddEnvironmentVariables();
        
            if (addUserSecrets)
            {
                // requires adding Microsoft.Extensions.Configuration.UserSecrets from NuGet.
                //builder.AddUserSecrets();
            }
        
            return builder.Build();
        }

        public static IConfigurationRoot GetConfiguration()
        {
            return GetConfiguration(Directory.GetCurrentDirectory());
        }
    }
}