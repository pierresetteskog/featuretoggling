using System;

namespace featuretoggling.Services
{
    public class TemperatureOnlyFreezingFeatureService : ITemperatureService
    {
        protected Random _random = new Random();

        public int GetTemperature()
        {
            return _random.Next(-200, -100);
        }
    }
}