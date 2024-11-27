using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MultiTenantJwtBearer.Contracts
{
    public interface ITenantJwtBearerConfigurationService
    {
        JwtBearerOptions GetJwtBearerOptions(string tenantId);
    }
}