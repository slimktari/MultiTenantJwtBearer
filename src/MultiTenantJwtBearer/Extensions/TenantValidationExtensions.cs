using System.Text.RegularExpressions;

namespace TeleworkingAssistant.WebApi.Identity
{
    public static partial class TenantValidationExtensions
    {
        /// <summary>
        /// Gets a compiled regex that validates a tenant's name.
        /// Names must start with a letter and can include letters, digits, and dashes.
        /// </summary>
        [GeneratedRegex("^[a-zA-Z][a-zA-Z0-9-]*$")]
        private static partial Regex TenantNamePattern();

        public static bool IsValidTenantName(this string? tenantName)
        {
            return !string.IsNullOrWhiteSpace(tenantName) &&
                   tenantName.Length >= 3 &&
                   TenantNamePattern().IsMatch(tenantName);
        }
    }
}