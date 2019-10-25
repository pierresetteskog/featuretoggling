using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace featuretoggling.FeatureFilters
{
    [FilterAlias("VCRS.Tenant")]
    public class TenantFilter : IFeatureFilter
    {
        private const string Alias = "VCRS.Tenant";

        private readonly ILogger _logger;
        private readonly ITenantResolver _tenantResolver;

        public TenantFilter(ILogger<TenantFilter> logger, ITenantResolver tenantResolver)
        {
            _logger = logger;
            _tenantResolver = tenantResolver;
        }

        public bool Evaluate(FeatureFilterEvaluationContext context)
        {
            var tenantSetting = context.Parameters.Get<TenantSettings>() ?? new TenantSettings();
            return tenantSetting.GetTenants().Contains(_tenantResolver.GetCurrentTenant() ?? "NULL");
        }
    }
}