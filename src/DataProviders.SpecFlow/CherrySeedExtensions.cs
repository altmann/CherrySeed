using CherrySeed.Configuration.Exceptions;
using TechTalk.SpecFlow;

namespace CherrySeed.DataProviders.SpecFlow
{
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
}