namespace MultiTenantJwtBearer.MultiTenancy.TenantName
{
    public interface ITenantNameAccessor
    {
        void SetTenantName(string tenantName);
        string GetTenantName();
    }
}