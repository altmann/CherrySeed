using CherrySeed.Configuration;
using CherrySeed.DataProviders.SpecFlow.Test.Entities;

namespace CherrySeed.DataProviders.SpecFlow.Test.Common
{
    public static class CherrySeedConfigurationExtensions
    {
        public static void WithCountryAndProjectEntities(this ISeederConfigurationBuilder cfg)
        {
            cfg.ForEntity<Country>()
                .WithPrimaryKey(e => e.Id);

            cfg.ForEntity<Project>()
                .WithPrimaryKey(e => e.Id)
                .WithReference(e => e.CountryId, typeof(Country));
        }
    }
}