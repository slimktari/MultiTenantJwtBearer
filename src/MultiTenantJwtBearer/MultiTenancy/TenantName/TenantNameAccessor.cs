using MultiTenantJwtBearer.Middleware;

namespace MultiTenantJwtBearer.MultiTenancy.TenantName
{
    internal class TenantNameAccessor(IHttpContextAccessor httpContextAccessor) : ITenantNameAccessor
    {
        public void SetTenantName(string tenantName)
        {
            if (httpContextAccessor.HttpContext == null)
            {
                throw new InvalidOperationException("HttpContext is null.");
            }
            httpContextAccessor.HttpContext.Items[HttpContextConstants.TenantNameKey] = tenantName;
        }

        public string GetTenantName()
        {
            var tenantName = httpContextAccessor.HttpContext?.Items[HttpContextConstants.TenantNameKey] as string;
            return tenantName ?? throw new InvalidOperationException("Tenant name is null.");
        }
    }

}