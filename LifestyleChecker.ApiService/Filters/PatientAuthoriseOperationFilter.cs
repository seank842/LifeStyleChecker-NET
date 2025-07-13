using LifestyleChecker.ApiService.Attrbutes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LifestyleChecker.ApiService.Filters
{
    public class PatientAuthoriseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAdminAuth = context.MethodInfo.GetCustomAttributes(true).OfType<PatientAuthoriseAttribute>().Any() ||
                           (context.MethodInfo.DeclaringType != null &&
                            context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<PatientAuthoriseAttribute>().Any());

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
                                Id = "basic Patient"
                            }
                        }
                    ] = Array.Empty<string>()
                }
            ];
            }
        }
    }
}
