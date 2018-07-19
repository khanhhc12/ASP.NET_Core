
using System.Collections.Generic;
using HelloSwaggerUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HelloSwaggerUI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SimpleController : ControllerBase
    {
        /// <summary>
        /// Simple summary
        /// </summary>
        /// <remarks>Simple remarks</remarks>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public ActionResult<List<SimpleModel>> ParseNoJsonProperty()
        {
            var listObj = new List<object>();
            for (int i = 1; i < 11; i++)
            {
                listObj.Add(new { id = i, name = "text" + i });
            }
            var resStr = JsonConvert.SerializeObject(listObj);
            var tokenObj = JToken.Parse(resStr);
            return tokenObj.ToObject<List<SimpleModel>>(new JsonSerializer { ContractResolver = new NoJsonPropertyContractResolver() });
            //return JsonConvert.DeserializeObject<List<SimpleModel>>(resStr, new JsonSerializerSettings { ContractResolver = new NoJsonPropertyContractResolver() });
        }
    }
}