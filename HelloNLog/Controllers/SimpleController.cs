using Microsoft.AspNetCore.Mvc;

namespace HelloNLog.Controllers
{
    public class SimpleController : Controller
    {
        [Route("api/simple/post")]
        public object Post([FromBody] SimpleModel model)
        {
            return model;
        }

        public class SimpleModel
        {
            public string value { get; set; }
            public string text { get; set; }
        }
    }
}