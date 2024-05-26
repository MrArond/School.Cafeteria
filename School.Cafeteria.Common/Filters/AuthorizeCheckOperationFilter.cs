using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace School.Cafeteria.Common.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                        .Union(context.MethodInfo.GetCustomAttributes(true))
                                        .OfType<AuthorizeAttribute>();

            if (authAttributes.Any())
            {
                operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = nameof(HttpStatusCode.Unauthorized) });
                operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = nameof(HttpStatusCode.Forbidden) });
            }

            if (authAttributes.Any())
            {
                operation.Security = new List<OpenApiSecurityRequirement>();

                var bearerSecurityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                };


                operation.Security.Add(new OpenApiSecurityRequirement()
                {
                    [bearerSecurityScheme] = new[] { "Bearer" }
                });
            }
        }
    }
}
