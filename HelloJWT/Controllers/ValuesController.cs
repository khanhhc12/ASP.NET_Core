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
        [Authorize]
        [HttpGet]
        public string Get()
        {
            var currentUser = HttpContext.User;

            if (currentUser.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                return currentUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                   .Select(c => c.Value).SingleOrDefault();
            }

            return "";
        }
    }
}