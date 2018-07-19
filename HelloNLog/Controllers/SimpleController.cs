using Microsoft.AspNetCore.Mvc;

namespace HelloNLog.Controllers
{
    [ApiController]
    public class SimpleController : ControllerBase
    {
        [Route("api/simple/post")]
        public object Post(SimpleModel model)
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