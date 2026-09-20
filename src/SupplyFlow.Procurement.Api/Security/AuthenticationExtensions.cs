using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SupplyFlow.Procurement.Api.Security;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authority =
            configuration["Authentication:Authority"]
            ?? throw new InvalidOperationException(
                "Authentication:Authority is not configured.");

        var audience =
            configuration["Authentication:Audience"]
            ?? throw new InvalidOperationException(
                "Authentication:Audience is not configured.");

        var metadataAddress =
            configuration["Authentication:MetadataAddress"];

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.MetadataAddress = metadataAddress;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        RoleClaimType = "roles"
                    };
            });

        services.AddAuthorization();

        return services;
    }
}