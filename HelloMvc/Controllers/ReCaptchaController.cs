using HelloMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelloMvc.Controllers
{
    public class ReCaptchaController : Controller
    {
        private static string site_key = "";
        private static string secret_key = "";
        public IActionResult Index()
        {
            ViewBag.site_key = site_key;
            return View();
        }

        // reCAPTHCA
        [HttpPost]
        public ContentResult SiteVerify([FromBody]ReCaptchaModel model)
        {
            string reqData = string.Format(
                "secret={0}&response={1}&remoteip={2}",
                secret_key,
                model.ReCaptchaResponse,
                Request.HttpContext.Connection.RemoteIpAddress.ToString()
            );
            string resJson = SimpleHttpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", reqData, "application/x-www-form-urlencoded").Result;
            return Content(resJson, "application/json");
        } 
    }
}