namespace MultiTenantJwtBearer.Entities
{
    public class TenantJwtConfig
    {
        public required string Authority { get; set; }
        public required string Audience { get; set; }
        public required string MetadataAddress { get; set; }
        public required string ClaimsIssuer { get; set; }
    }
}