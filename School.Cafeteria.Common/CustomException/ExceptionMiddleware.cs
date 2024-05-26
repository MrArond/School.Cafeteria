using Microsoft.AspNetCore.Http;
using School.Cafeteria.Models.Firebase;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace School.Cafeteria.Common.CustomException
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;

            this._jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (SchoolCafeteriaException error)
            {
                httpContext.Response.StatusCode = error.Error.Error.Code;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(error.Error, this._jsonSerializerOptions));
            }
            catch (Exception error)
            {
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";

                var firebaseError = new FirebaseError
                {
                    Error = new Error
                    {
                        Code = 500,
                        Message = error.Message,
                    }
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(firebaseError.Error, this._jsonSerializerOptions));
            }
        }
    }
}
