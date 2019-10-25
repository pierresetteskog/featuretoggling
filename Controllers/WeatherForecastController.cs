using System;
using System.Collections.Generic;
using System.Linq;
using featuretoggling.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace featuretoggling.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IFeatureManagerSnapshot _featureManager;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOptionsSnapshot<Settings> _settings;
        private readonly ITemperatureService _temperatureService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptionsSnapshot<Settings> settings,
            IFeatureManagerSnapshot featureManager
            , ITemperatureService temperatureService)
        {
            _settings = settings;
            _featureManager = featureManager;
            _temperatureService = temperatureService;
            _logger = logger;
        }


        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var numberOfDays = 5;
            if (_featureManager.IsEnabled(nameof(FeatureFlags.LongForecast))) numberOfDays = 10;
            return Enumerable.Range(1, numberOfDays).Select(index => new WeatherForecast
                {
                    Date = _featureManager.IsEnabled(nameof(FeatureFlags.NiceDates))
                        ? DateTime.Now.AddDays(index).Date
                        : DateTime.Now.AddDays(index),
                    TemperatureC = _temperatureService.GetTemperature(),
                    Summary =
                        $"{_settings.Value.SummaryTextIngress} {(_featureManager.IsEnabled(nameof(FeatureFlags.OnlyFreezing)) ? Summaries[0] : Summaries[rng.Next(Summaries.Length)])}"
                })
                .ToArray();
        }
    }
}