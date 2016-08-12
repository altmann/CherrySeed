using System;
using CherrySeed.EntitySettings;

namespace CherrySeed.Configuration
{
    public class SeederConfiguration
    {
        private readonly Action<ISeederConfigurationBuilder> _configurationExpression;

        public SeederConfiguration(Action<ISeederConfigurationBuilder> configurationExpression)
        {
            _configurationExpression = configurationExpression;
        }

        public ICherrySeeder CreateSeeder()
        {
            return new CherrySeeder(_configurationExpression);
        }
    }
}