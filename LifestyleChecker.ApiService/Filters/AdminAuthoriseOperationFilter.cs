using LifestyleChecker.ApiService.Attrbutes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LifestyleChecker.ApiService.Filters
{
    public class AdminAuthoriseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAdminAuth = context.MethodInfo.GetCustomAttributes(true).OfType<AdminAuthoriseAttribute>().Any() ||
                           (context.MethodInfo.DeclaringType != null &&
                            context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AdminAuthoriseAttribute>().Any());

            if (hasAdminAuth)
            {
                operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    [ new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic Admin"
                            }
                        }
                    ] = Array.Empty<string>()
                }
            ];
            }
        }
    }
}
