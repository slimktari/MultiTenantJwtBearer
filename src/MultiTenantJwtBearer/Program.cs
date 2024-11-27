using Microsoft.AspNetCore.Authentication.JwtBearer;
using MultiTenantJwtBearer.Contracts;
using MultiTenantJwtBearer.Extensions;
using MultiTenantJwtBearer.Middleware;
using MultiTenantJwtBearer.MultiTenancy.TenantName;
using MultiTenantJwtBearer.Services;

namespace MultiTenantJwtBearer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();
            
            builder.Services.AddTransient<ITenantNameAccessor, TenantNameAccessor>();
            builder.Services.AddScoped<ITenantNameProvider, TenantNameProvider>();
            builder.Services.AddScoped<ITenantJwtBearerConfigurationService, TenantJwtBearerConfigurationService>();

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
                .AddMultiTenantJwtBearer(mainSchemeHandler, options =>
                {   
                    // Setting the common options for all tenants.
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