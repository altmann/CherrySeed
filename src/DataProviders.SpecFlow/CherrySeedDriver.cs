using System;
using CherrySeed.Configuration;
using CherrySeed.Configuration.Exceptions;
using TechTalk.SpecFlow;

namespace CherrySeed.DataProviders.SpecFlow
{
    public static class CherrySeedConfigurationExtensions
    {
        public static void WithSpecFlowConfiguration(this ISeederConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.WithDataProvider(new SpecFlowDataProvider());
            configurationBuilder.DisableClearBeforeSeeding();
        }
    }

    public static class CherrySeedExtensions
    {
        public static void Seed(this ICherrySeeder seeder, string entityName, Table table)
        {
            var cherrySeeder = seeder as CherrySeeder;
            var dataProvider = cherrySeeder.DataProvider as SpecFlowDataProvider;

            if (dataProvider == null)
            {
                throw new ConfigurationException("CherrySeed has an uncorrect SpecFlow configuration. Call method WithSpecFlowConfiguration() in the CherrySeed configuration section.", null);
            }

            dataProvider.ClearAndAdd(entityName, table);
            seeder.Seed();
        }
    }



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