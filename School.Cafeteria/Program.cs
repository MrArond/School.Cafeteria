using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using School.Cafeteria.Business.Handlers;
using School.Cafeteria.Business.HttpClients;
using School.Cafeteria.Common.CustomException;
using School.Cafeteria.Common.Filters;
using School.Cafeteria.Domain;
using School.Cafeteria.Domain.Abstract;
using School.Cafeteria.Domain.Concrete;
using School.Cafeteria.Models.Firebase;
using School.Cafeteria.Services.Interfaces;
using School.Cafeteria.Services.Services;

namespace School.Cafeteria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
                options.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "School Cafeteria",
                    Version = "v1",
                    Description = "School Cafeteria API"
                });

                c.DescribeAllParametersInCamelCase();
                c.EnableAnnotations();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Firebase Token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.OrderActionsBy(o => $"{o.ActionDescriptor.RouteValues["controller"]}_{HttpMethodToRank(o.HttpMethod)}");
            });

            builder.Services.AddOptions<FirebaseConfig>().BindConfiguration("FirebaseConfig");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://securetoken.google.com/schoolcafeteria-4bc8f";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/schoolcafeteria-4bc8f",
                    ValidateAudience = true,
                    ValidAudience = "schoolcafeteria-4bc8f",
                    ValidateLifetime = true
                };
                options.RequireHttpsMetadata = false;
            });

            builder.Services.AddDbContext<EFDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
            });

            builder.Services.AddTransient<IEFAccountRepository, EFAccountRepository>();
            builder.Services.AddTransient<IEFProductRepository, EFProductRepository>();

            builder.Services.AddTransient<FirebaseKeyHandler>();

            var firebaseConfig = builder.Configuration.GetSection("FirebaseConfig").Get<FirebaseConfig>();
            builder.Services.AddHttpClient("Firebase", config =>
            {
                config.BaseAddress = new Uri(firebaseConfig.Path);
            })
                .AddHttpMessageHandler<FirebaseKeyHandler>();

            builder.Services.AddSingleton(sp =>
            {
                var factory = sp.GetService<IHttpClientFactory>();
                var httpClient = factory.CreateClient("Firebase");

                return new FirebaseClient(httpClient);
            });

            builder.Services.AddScoped<IFirebaseService, FirebaseService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static int HttpMethodToRank(string httpMethod)
        {
            return httpMethod?.ToUpper() switch
            {
                "GET" => 1,
                "POST" => 2,
                "PUT" => 3,
                "PATCH" => 4,
                "DELETE" => 5,
                _ => 9
            };
        }
    }
}
