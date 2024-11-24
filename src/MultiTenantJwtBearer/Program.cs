using Microsoft.AspNetCore.Authentication.JwtBearer;
using MultiTenantJwtBearer.Contracts;
using MultiTenantJwtBearer.MultiTenancy;

namespace MultiTenantJwtBearer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<ITenantNameProvider, TenantNameProvider>();

            string authority = builder.Configuration.GetValue<string>("Authentication:Schemes:Bearer:Authority")!;
            string audience = builder.Configuration.GetValue<string>("Authentication:Schemes:Bearer:Audience")!;
            bool requireHttpsMetadata = builder.Configuration.GetValue<bool>("Authentication:Schemes:Bearer:RequireHttpsMetadata");
            bool useSecurityTokenValidators = builder.Configuration.GetValue<bool>("Authentication:Schemes:Bearer:UseSecurityTokenValidators");

            var mainSchemeHandler = JwtBearerDefaults.AuthenticationScheme;
            builder.Services
                .AddAuthentication(
                    options =>
                    {
                        // Setting the main scheme as the default is necessary for the challenge scheme to work correctly.
                        options.DefaultAuthenticateScheme = mainSchemeHandler;
                        options.DefaultChallengeScheme = mainSchemeHandler;
                        options.DefaultScheme = mainSchemeHandler;
                        options.DefaultForbidScheme = mainSchemeHandler;
                    })
                .AddMultiTenantJwtBearerToken(mainSchemeHandler, options =>
                {
                    // Custom options for multi-tenant JWT validation.
                    options.Audience = audience;
                    options.Authority = authority;
                    options.RequireHttpsMetadata = requireHttpsMetadata;
                    options.UseSecurityTokenValidators = useSecurityTokenValidators;
                });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();
            
            app.UseMiddleware<TenantNameMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}