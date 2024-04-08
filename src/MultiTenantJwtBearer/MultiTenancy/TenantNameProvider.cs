using MultiTenantJwtBearer.Contracts;

namespace MultiTenantJwtBearer.MultiTenancy
{
    public class TenantNameProvider(IHttpContextAccessor httpContextAccessor) : ITenantNameProvider
    {
        public string GetCurrentTenantName()
        {
            return httpContextAccessor.HttpContext?.Items[HttpContextConstants.TenantNameKey] as string ?? throw new InvalidOperationException("Tenant name is null.");
        }
    }
}