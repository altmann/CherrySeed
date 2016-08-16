using System;

namespace CherrySeed.Configuration
{
    public class CherrySeedConfiguration
    {
        private readonly Action<ISeederConfigurationBuilder> _configurationExpression;

        public CherrySeedConfiguration(Action<ISeederConfigurationBuilder> configurationExpression)
        {
            _configurationExpression = configurationExpression;
        }

        public ICherrySeeder CreateSeeder()
        {
            return new CherrySeeder(_configurationExpression);
        }
    }
}