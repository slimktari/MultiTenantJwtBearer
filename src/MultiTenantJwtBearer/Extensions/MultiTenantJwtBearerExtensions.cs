using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MultiTenantJwtBearer.MultiTenancy.Handlers;

namespace MultiTenantJwtBearer.Extensions
{
    public static class MultiTenantJwtBearerExtensions
    {
        public static AuthenticationBuilder AddMultiTenantJwtBearer(
            this AuthenticationBuilder builder) => builder.AddMultiTenantJwtBearer(JwtBearerDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddMultiTenantJwtBearer(
            this AuthenticationBuilder builder, string authenticationScheme) => builder.AddMultiTenantJwtBearer(authenticationScheme, _ => { });

        public static AuthenticationBuilder AddMultiTenantJwtBearer(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<JwtBearerOptions> configureOptions)
        {
            builder.Services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>();
            return builder.AddScheme<JwtBearerOptions, MultiTenantJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, configureOptions);
        }
    }
}