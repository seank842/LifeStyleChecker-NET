using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace LifestyleChecker.ApiService.Attrbutes
{
    /// <summary>  
    /// An authorization filter attribute that restricts access to actions to users with valid admin credentials.  
    /// </summary>  
    /// <remarks>This attribute checks for a Basic Authentication header in the HTTP request and validates the  
    /// credentials against the admin username and password specified in the application's configuration. If the  
    /// credentials are invalid or missing, the request is denied with a 401 Unauthorized response.</remarks>  
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminAuthoriseAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Handles authorization by validating the Basic authentication credentials against configured admin
        /// credentials.
        /// </summary>
        /// <remarks>This method checks for the presence of a Basic authentication header in the request.
        /// It decodes the credentials and compares them against the admin username and password specified in the
        /// configuration section "AdminCredentials". If the credentials are valid, the request is allowed to proceed.
        /// Otherwise, it sets the result to <see cref="UnauthorizedResult"/> and adds a "WWW-Authenticate" header to
        /// the response, prompting the client for credentials.</remarks>
        /// <param name="context">The <see cref="AuthorizationFilterContext"/> containing the HTTP context and other information about the
        /// request.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) is not IConfiguration config)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var adminSection = config.GetSection("AdminCredentials");
            var adminUser = adminSection["Username"];
            var adminPass = adminSection["Password"];

            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                var encoded = authHeader.Substring("Basic ".Length).Trim();
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                var parts = decoded.Split(':', 2);
                if (parts.Length == 2 && parts[0] == adminUser && parts[1] == adminPass)
                {
                    return;
                }
            }

            context.HttpContext.Response.Headers.WWWAuthenticate = "Basic";
            context.Result = new UnauthorizedResult();
        }
    }
}
