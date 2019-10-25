using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;

namespace featuretoggling
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                #region added

                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var settings = config.Build();

                    config.AddAzureAppConfiguration(options =>
                    {
                        options.Connect(settings["ConnectionStrings:AppConfig"])
                            .ConfigureRefresh(refresh =>
                            {
                                refresh.Register("TestApp:Settings:SummaryTextIngress");
                                refresh.SetCacheExpiration(new TimeSpan(0, 0, 5));
                            })
                            .Use(KeyFilter.Any, "")
                            .UseFeatureFlags();
                    });
                    settings = config.Build();
                })

                #endregion

                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}