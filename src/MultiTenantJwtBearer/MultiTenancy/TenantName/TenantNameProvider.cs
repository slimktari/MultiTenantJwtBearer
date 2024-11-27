using MultiTenantJwtBearer.Contracts;

namespace MultiTenantJwtBearer.MultiTenancy.TenantName
{
    public class TenantNameProvider(ITenantNameAccessor tenantNameAccessor) : ITenantNameProvider
    {
        public string GetTenantName()
        {
            return tenantNameAccessor.GetTenantName();
        }
    }
}