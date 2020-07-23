using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        [Authorize(Roles = AuthorizeRole.Admin)]
        [HttpGet]
        public object Get()
        {
            var currentUser = HttpContext.User;
            return currentUser.Claims.Select(c => new { Type = c.Type, Value = c.Value });
        }
    }
}