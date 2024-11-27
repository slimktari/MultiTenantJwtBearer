using Microsoft.AspNetCore.Authentication.JwtBearer;
using MultiTenantJwtBearer.Contracts;
using MultiTenantJwtBearer.Entities;

namespace MultiTenantJwtBearer.Services;

public class TenantJwtBearerConfigurationService : ITenantJwtBearerConfigurationService
{
    public JwtBearerOptions GetJwtBearerOptions(string tenantId)
    {
        if (!TenantConfigurations.TryGetValue(tenantId, out var tenantConfig))
        {
            throw new KeyNotFoundException($"Configuration for tenant '{tenantId}' not found.");
        }

        return new JwtBearerOptions
        {
            Authority = tenantConfig.Authority,
            Audience = tenantConfig.Audience,
            MetadataAddress = tenantConfig.MetadataAddress,
            ClaimsIssuer = tenantConfig.ClaimsIssuer,
            
            // Other options...
        };
    }

    // Fake data.
    private static readonly Dictionary<string, TenantJwtConfig> TenantConfigurations = new()
    {
        {
            "Tenant1", new TenantJwtConfig
            {
                Authority = "https://auth.tenant1.com/",
                Audience = "api",
                MetadataAddress = "https://auth.tenant1.com/tenant1/.well-known/openid-configuration",
                ClaimsIssuer = "https://claims.tenant1.com/"
            }
        },
        {
            "Tenant2", new TenantJwtConfig
            {
                Authority = "https://auth.tenant2.com/",
                Audience = "api",
                MetadataAddress = "https://auth.tenant2.com/tenant2/.well-known/openid-configuration",
                ClaimsIssuer = "https://claims.tenant2.com/"
            }
        }
    };
}