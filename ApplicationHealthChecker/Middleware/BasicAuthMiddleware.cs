using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApplicationHealthChecker.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ApplicationHealthChecker.Middleware
{
    public class BasicAuthMiddleware
    {

        private readonly RequestDelegate next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IOptions<BasicAuthSettings> swaggerAuthSettings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (swaggerAuthSettings == null)
            {
                throw new ArgumentNullException(nameof(swaggerAuthSettings));
            }

            var settings = swaggerAuthSettings.Value;

            //Make sure we are hitting the swagger path, and not doing it locally as it just gets annoying :-)
            if (context.Request.Path.StartsWithSegments("/healthchecks-ui", StringComparison.Ordinal))
            {
                if (!settings.Enabled)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                if (!settings.RequireAuth)
                {
                    await this.next.Invoke(context);
                    return;
                }

                string authHeader = context.Request.Headers["Authorization"];
                if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Basic ", StringComparison.Ordinal))
                {
                    // Get the encoded username and password
                    var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                    // Decode from Base64 to string
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword ?? ""));

                    // Split username and password
                    var username = decodedUsernamePassword.Split(':', 2)[0];
                    var password = decodedUsernamePassword.Split(':', 2)[1];

                    // Check if login is correct
                    if (IsAuthorized(username, password, settings))
                    {
                        await this.next.Invoke(context);
                        return;
                    }
                }

                // Return authentication type (causes browser to show login dialog)
                context.Response.Headers["WWW-Authenticate"] = "Basic";

                // Return unauthorized
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await this.next.Invoke(context);
            }
        }

        public static bool IsAuthorized(string username, string password, BasicAuthSettings settings)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            // Check that username and password are correct
            return username.Equals(settings.Username, StringComparison.OrdinalIgnoreCase) && password.Equals(settings.Password, StringComparison.Ordinal);
        }
    }
}