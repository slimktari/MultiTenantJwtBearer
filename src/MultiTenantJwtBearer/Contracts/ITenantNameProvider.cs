namespace MultiTenantJwtBearer.Contracts
{
    public interface ITenantNameProvider
    {
        string GetTenantName();
    }
}