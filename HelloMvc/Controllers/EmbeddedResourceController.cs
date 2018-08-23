using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace HelloMvc.Controllers
{
    public class EmbeddedResourceController : Controller
    {
        // /css/site.css/
        // /js/site.js/
        [Route("{folder1}/{fileName}")]
        [Route("{folder1}/{folder2}/{fileName}")]
        public ContentResult Read(string folder1, string folder2, string fileName)
        {
            string file = "";
            var assembly = Assembly.GetExecutingAssembly();
            string name = assembly.EntryPoint.DeclaringType.Namespace + ".wwwroot." + folder1 + (string.IsNullOrEmpty(folder2) ? "" : "." + folder2) + "." + fileName;
            string extension = Path.GetExtension(fileName);
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (stream != null)
                    using (var reader = new StreamReader(stream))
                        file = reader.ReadToEnd();
            }
            return Content(file, extension == ".css" ? "text/css" : (extension == ".js" ? "application/javascript" : "text/plain"));
        }
    }
}