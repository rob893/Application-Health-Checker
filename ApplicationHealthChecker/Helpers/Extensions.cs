using HealthChecker.Middleware;
using Microsoft.AspNetCore.Builder;

namespace HealthChecker.Helpers
{
    public static class Extensions
    {
        public static IApplicationBuilder UseBasicAuthForUI(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthMiddleware>();
        }
    }
}