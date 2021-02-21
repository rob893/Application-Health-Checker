using ApplicationHealthChecker.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ApplicationHealthChecker.Helpers
{
    public static class Extensions
    {
        public static IApplicationBuilder UseBasicAuthForUI(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthMiddleware>();
        }
    }
}