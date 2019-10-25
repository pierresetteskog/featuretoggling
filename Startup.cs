using featuretoggling.FeatureFilters;
using featuretoggling.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

namespace featuretoggling
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region added

            services.AddFeatureManagement()
                .AddFeatureFilter<PercentageFilter>()
                .AddFeatureFilter<TenantFilter>();
            services.Configure<Settings>(Configuration.GetSection("TestApp:Settings"));
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #endregion

            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddTransient<ITenantResolver, TenantResolver>();
            services.AddTransient<TemperatureService, TemperatureService>();
            services.AddTransient<TemperatureOnlyFreezingFeatureService, TemperatureOnlyFreezingFeatureService>();
            services.AddTransient(c =>
                c.GetRequiredService<IFeatureManagerSnapshot>().IsEnabled(nameof(FeatureFlags.OnlyFreezing))
                    ? c.GetRequiredService<TemperatureOnlyFreezingFeatureService>()
                    : (ITemperatureService)c.GetRequiredService<TemperatureService>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region added

            app.UseAzureAppConfiguration();

            #endregion

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}