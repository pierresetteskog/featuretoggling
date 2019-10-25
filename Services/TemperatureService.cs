using System;

namespace featuretoggling.Services
{
    public class TemperatureService : ITemperatureService
    {
        protected Random _random = new Random();

        public int GetTemperature()
        {
            return _random.Next(-20, 55);
        }
    }
}