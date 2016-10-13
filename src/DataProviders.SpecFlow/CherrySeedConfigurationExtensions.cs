using CherrySeed.Configuration;

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
}