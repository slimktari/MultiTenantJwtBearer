# MultiTenantJwtBearer

## Overview

`MultiTenantJwtBearer` is an extension for ASP.NET Core applications, designed to support JWT token validation in a multi-tenant setup, particularly where tenancy is determined via the URL. This project extends the native `AddJwtBearer()` method to allow for easy multi-tenancy integration without the need for extensive code modifications or forking. The aim is to ensure seamless updates to dependencies like `Microsoft.Identity.Web`, enhancing maintainability and future-proofing your application.

## Key Features

- **Multi-Tenant JWT Validation:** Streamlines JWT token validation across multiple tenants, identified by URL, in ASP.NET Core APIs.
- **Minimal Code Changes:** Leverages the existing `AddJwtBearer()` method, requiring minimal adjustments for multi-tenant support.
- **Future-Proof:** Designed for compatibility with updates to core dependencies, ensuring long-term viability.

## Project Status

Currently, `MultiTenantJwtBearer` is in an experimental phase. It focuses on the specific use case of validating JWT tokens for API requests formatted as `https://host/tenantName/route`, without preloading all tenant configurations. Users are advised to conduct thorough testing before incorporating this into production environments.

## Usage

Integrate `MultiTenantJwtBearer` into your project by extending the `AddJwtBearer()` method with multi-tenant support as shown below:

```csharp
var mainSchemeHandler = JwtBearerDefaults.AuthenticationScheme;
builder.Services.AddAuthentication(options =>
    {
        // Setting the main scheme as the default is necessary for the challenge scheme to function correctly.
        options.DefaultAuthenticateScheme = mainSchemeHandler;
        options.DefaultChallengeScheme = mainSchemeHandler;
        options.DefaultScheme = mainSchemeHandler;
        options.DefaultForbidScheme = mainSchemeHandler;
    })
    .AddMultiTenantJwtBearer(mainSchemeHandler, options =>
    {
        // Custom options for multi-tenant JWT validation
        options.Audience = "YourAudience";
        options.Authority = "https://yourauthority.com";
        options.RequireHttpsMetadata = true;
        options.UseSecurityTokenValidators = true;
    });

    // Feel free to add additional options here. All specified options will be mapped and adapted for each tenant.
```

Replace the placeholder values (YourAudience, https://yourauthority.com, etc.) with your specific configuration details.

## Feedback and Contributions

Feedback, suggestions, and contributions are highly welcome. If you have any ideas, encounter issues, or want to contribute, please open an issue or submit a pull request. Your input helps make MultiTenantJwtBearer better for everyone.