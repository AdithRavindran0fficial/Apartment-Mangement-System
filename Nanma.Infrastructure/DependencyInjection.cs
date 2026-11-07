using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nanma.Application.Common.Abstractions;
using Nanma.Infrastructure.Auth;
using Nanma.Infrastructure.Persistence;
using Nanma.Infrastructure.Repositories;
using Nanma.Infrastructure.Time;

namespace Nanma.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

        services.AddDbContext<NanmaDbContext>(options =>
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IClock, SystemClock>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}
