using Application.Common.Interfaces;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<Application.Common.Interfaces.ITokenService, JwtTokenService>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddInfrastructureServices(config);

        // JWT HS256 validation
        var jwtKey = config["Jwt:Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = config["Jwt:Audience"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Api.Read", p => p.RequireAuthenticatedUser());
            options.AddPolicy("Api.Write", p => p.RequireAuthenticatedUser());
        });

        return services;
    }
}