using System;
using CherrySeed.Configuration;
using TechTalk.SpecFlow;

namespace CherrySeed.DataProviders.SpecFlow
{
    public class CherrySeedDriver
    {
        private readonly ICherrySeeder _cherrySeeder;
        private readonly SpecFlowDataProvider _dataProvider;

        public CherrySeedDriver(Action<ISeederConfigurationBuilder> configurationExpression)
        {
            _dataProvider = new SpecFlowDataProvider();

            var cherrySeedConfiguration = new CherrySeedConfiguration(cfg =>
            {
                // set default settings
                cfg.DisableClearBeforeSeeding();
                cfg.WithDataProvider(_dataProvider);

                // set custom settings
                configurationExpression(cfg);
            });

            _cherrySeeder = cherrySeedConfiguration.CreateSeeder();
        }

        public void Seed(string entityName, Table table)
        {
            _dataProvider.ClearAndAdd(entityName, table);
            _cherrySeeder.Seed();
        }

        public void Clear()
        {
            _cherrySeeder.Clear();
        }
    }
}