using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HelloNLog.Controllers
{
    [ApiController]
    public class SimpleController : ControllerBase
    {
        [HttpPost]
        [Route("api/simple/post")]
        public object Post(SimpleModel model)
        {
            return model;
        }

        public class SimpleModel
        {
            public string value { get; set; }
            public string text { get; set; }
            [JsonIgnore]
            public string ignore { get; set; }
        }
    }
}