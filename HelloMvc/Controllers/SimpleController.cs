using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HelloMvc.Controllers
{
    public class SimpleController : Controller
    {
        // JsonStreamingResult
        public void Index()
        {
            var itemToSerialize = new List<Guid>();
            for (int i = 0; i < 100000; i++)
                itemToSerialize.Add(Guid.NewGuid());
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            using (Response.Body)
            {
                JsonSerializer serializer = new JsonSerializer();
                using (var sw = new StreamWriter(Response.Body))
                using (JsonTextWriter writer = new JsonTextWriter(sw))
                {
                    JToken obj = JToken.FromObject(itemToSerialize, serializer);
                    obj.WriteTo(writer);
                    writer.Flush();
                }
            }
        }
    }
}