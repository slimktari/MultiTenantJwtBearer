using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace MultiTenantJwtBearer.MultiTenancy
{
    public static class MultiTenantJwtBearerExtensions
    {
        public static AuthenticationBuilder AddMultiTenantJwtBearerToken(
            this AuthenticationBuilder builder, 
            string authenticationScheme, 
            Action<JwtBearerOptions> configureOptions)
        {
            builder.Services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>();
            return builder.AddScheme<JwtBearerOptions, MultiTenantJwtBearerHandler>(authenticationScheme, configureOptions);
        }
    }
}