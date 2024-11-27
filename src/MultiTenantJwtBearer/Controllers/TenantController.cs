using Microsoft.AspNetCore.Mvc;
using MultiTenantJwtBearer.Contracts;

namespace MultiTenantJwtBearer.Controllers
{
    [ApiController]
    [Route("{TenantName}/[controller]")]
    public class TenantController(ITenantNameProvider tenantNameProvider) : ControllerBase
    {
        [HttpGet("whoami")]
        public IActionResult GetTenantName()
        {
            var tenantName = tenantNameProvider.GetTenantName();

            if (string.IsNullOrWhiteSpace(tenantName))
            {
                return BadRequest(new { message = "Tenant name not provided." });
            }

            return Ok(new { tenantName });
        }
    }
}