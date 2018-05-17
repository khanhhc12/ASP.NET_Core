using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HelloSwaggerUI.Models
{
    public class SwaggerAuthorizedMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerAuthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger") && CheckAuthorized(context))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next.Invoke(context);
        }

        public static bool CheckAuthorized(HttpContext context)
        {
            var remoteIpAddress = context.Connection.RemoteIpAddress.ToString();
            var localIpAddress = context.Connection.LocalIpAddress.ToString();
            return false;
        }
    }
}