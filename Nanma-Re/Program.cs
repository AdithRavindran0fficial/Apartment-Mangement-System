using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Nanma.Application;
using Nanma.Infrastructure;
using Nanma_Re.Middleware;
using Serilog;

namespace Nanma_Re
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddOpenApi();

            builder.Services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MobileClient", policy =>
                {
                    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

                    if (allowedOrigins.Length == 0)
                    {
                        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                        return;
                    }

                    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
                });
            });

            var jwtSection = builder.Configuration.GetSection("Jwt");
            var signingKey = jwtSection["SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey is missing.");
            var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
            var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing.");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1),
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseSerilogRequestLogging();
            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors("MobileClient");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
