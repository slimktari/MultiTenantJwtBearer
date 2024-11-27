using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MultiTenantJwtBearer.Contracts;

namespace MultiTenantJwtBearer.MultiTenancy.Handlers
{
    /// <summary>
    /// A custom JwtBearerHandler that supports tenant-specific configurations.
    /// </summary>
    public class MultiTenantJwtBearerHandler(
        IOptionsMonitor<JwtBearerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IEnumerable<IPostConfigureOptions<JwtBearerOptions>> postConfigures,
        IOptionsMonitorCache<JwtBearerOptions> cache,
        IAuthenticationSchemeProvider schemes,
        ITenantNameProvider tenantNameProvider,
        ITenantJwtBearerConfigurationService tenantConfigurationService
        )
        : JwtBearerHandler(options, logger, encoder)
    {
        /// <summary>
        /// Handles the authentication asynchronously by considering tenant-specific configurations.
        /// </summary>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var tenantName = tenantNameProvider.GetTenantName();
            if (string.IsNullOrEmpty(tenantName))
            {
                return AuthenticateResult.Fail("The tenant information is not valid or missing.");
            }
            try
            {
                // Ensures the tenant authentication scheme and options are available in IOptionsMonitorCache or creates them if necessary.
                EnsureTenantOptions(tenantName);
                await EnsureTenantScheme(tenantName);
                return await Context.AuthenticateAsync(tenantName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while handling tenant authentication for {TenantName}.", tenantName);
                return AuthenticateResult.Fail("An error occurred during authentication.");
            }
        }

        /// <summary>
        /// Ensures the tenant-specific options are available in IOptionsMonitorCache or creates them if necessary.
        /// Tenant-specific options must be cached for handler initialization.
        /// </summary>
        private void EnsureTenantOptions(string tenantName)
        {
            cache.GetOrAdd(tenantName, () =>
            {
                var options = InitTenantOptions(tenantName);
                PostConfigure(tenantName, options);
                return options;
            });
        }

        /// <summary>
        /// Use the tenant name and default options to create a new instance of <see cref="JwtBearerOptions"/>.
        /// </summary>
        private JwtBearerOptions InitTenantOptions(string tenantName)
        {
            var tenantConfiguration = tenantConfigurationService.GetJwtBearerOptions(tenantName);
            return new JwtBearerOptions
            {
                // Set specific configuration.
                Audience = tenantConfiguration.Audience,
                Authority = tenantConfiguration.Authority,
                MetadataAddress = tenantConfiguration.MetadataAddress,
                ClaimsIssuer = tenantConfiguration.ClaimsIssuer,

                // Set common configuration. Depends on your needs, you can move options to ITenantJwtBearerConfigurationService.
                Events = Options.Events,
                EventsType = Options.EventsType,

                SaveToken = Options.SaveToken,
                MapInboundClaims = Options.MapInboundClaims,
                IncludeErrorDetails = Options.IncludeErrorDetails,
                RequireHttpsMetadata = Options.RequireHttpsMetadata,
                UseSecurityTokenValidators = Options.UseSecurityTokenValidators,
                RefreshOnIssuerKeyNotFound = Options.RefreshOnIssuerKeyNotFound,

                TimeProvider = Options.TimeProvider,
                AutomaticRefreshInterval = Options.AutomaticRefreshInterval,
                TokenValidationParameters = Options.TokenValidationParameters.Clone(),

                Backchannel = Options.Backchannel,
                BackchannelTimeout = Options.BackchannelTimeout,
                BackchannelHttpHandler = Options.BackchannelHttpHandler,

                Challenge = Options.Challenge,
                ForwardAuthenticate = Options.ForwardAuthenticate,
                ForwardChallenge = Options.ForwardChallenge,
                ForwardDefault = Options.ForwardDefault,
                ForwardForbid = Options.ForwardForbid,
                ForwardSignIn = Options.ForwardSignIn,
                ForwardSignOut = Options.ForwardSignOut,
                ForwardDefaultSelector = Options.ForwardDefaultSelector
            };
        }

        /// <summary>
        /// Applies post-configuration adjustments to the tenant-specific options.
        /// Post-configuration is used to apply additional settings to the options like ConfigurationManager.
        /// </summary>
        private void PostConfigure(string tenantName, JwtBearerOptions options)
        {
            foreach (var postConfigure in postConfigures)
            {
                postConfigure.PostConfigure(tenantName, options);
            }
        }

        /// <summary>
        /// Ensures the shared authentication scheme for the tenant exists or creates it.
        /// Authentication uses a tenant-specific scheme to initialize a new <see cref="JwtBearerHandler"/>> per the scheme's handlerType.
        /// </summary>
        private async Task EnsureTenantScheme(string tenantName)
        {
            var tenantScheme = await schemes.GetSchemeAsync(tenantName);
            if (tenantScheme == null)
            {
                tenantScheme = new AuthenticationScheme(
                    tenantName,
                    tenantName, typeof(JwtBearerHandler));
                schemes.TryAddScheme(tenantScheme);
            }
        }
    }
}