using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace HelloMvc.Controllers
{
    public class StreamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public void Test()
        {
            var itemToSerialize = new List<Guid>();
            for (int i = 0; i < 100000; i++)
                itemToSerialize.Add(Guid.NewGuid());
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            using (Response.Body)
            {
                JsonSerializer.SerializeAsync(Response.Body, itemToSerialize);
            }
        }
    }
}
