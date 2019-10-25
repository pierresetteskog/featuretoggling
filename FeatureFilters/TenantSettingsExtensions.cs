using System;
using System.Collections.Generic;
using System.Linq;

public static class TenantSettingsExtensions
{
    public static List<string> GetTenants(this TenantSettings settings)
    {
        if (!string.IsNullOrWhiteSpace(settings?.Value))
            return settings.Value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).ToList();
        return new List<string>();
    }
}