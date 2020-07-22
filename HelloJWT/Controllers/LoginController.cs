using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HelloJWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HelloJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public object Login([FromBody] UserModel login)
        {
            if (login != null && login.Username != null)
                return new { token = CreateToken(login.Username) };
            return null;
        }

        private string CreateToken(string Username)
        {
            //Set issued at date
            DateTime notBefore = DateTime.Now;
            //set the time when it expires
            DateTime expires = DateTime.Now.AddMinutes(120);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //create a identity and add claims to the user which we want to log in
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, Username)
            };

            //Create the jwt (JSON Web Token)
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                notBefore,
                expires,
                credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}