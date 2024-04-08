namespace MultiTenantJwtBearer.MultiTenancy;

public class TenantNameMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Extract tenantName from the request path.
        // Assuming the tenantName is the first segment of the path (e.g., /alpha-corp/api/resource).
        var path = context.Request.Path.Value;
        var segments = path?.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments is { Length: > 0 })
        {
            var tenantName = segments[0]; // The first segment is assumed to be the tenantName.

            // Validate the tenantName.
            if (tenantName.IsValidTenantName())
            {
                // If valid, store it in HttpContext.Items.
                context.Items[HttpContextConstants.TenantNameKey] = tenantName;
            }
            else
            {
                // If not valid, handle accordingly (e.g., respond with an error).
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid tenant name.");
                return;
            }
        }

        // Call the next middleware in the pipeline.
        await next(context);
    }
}