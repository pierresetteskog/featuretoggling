using Microsoft.AspNetCore.Http;

namespace featuretoggling.FeatureFilters
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IHttpContextAccessor _httpContext;

        public TenantResolver(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetCurrentTenant()
        {
            return _httpContext.HttpContext.Request.Query["tenantid"];
        }
    }
}