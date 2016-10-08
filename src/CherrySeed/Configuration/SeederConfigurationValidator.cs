using CherrySeed.Configuration.Exceptions;

namespace CherrySeed.Configuration
{
    public class SeederConfigurationValidator
    {
        public void IsValid(SeederConfiguration configuration)
        {
            IsDataProviderValid(configuration);
            IsRepositoryValid(configuration);
        }

        private static void IsRepositoryValid(SeederConfiguration configuration)
        {
            if (configuration.DefaultRepository == null)
            {
                throw new MissingConfigurationException("Repository");
            }
        }

        private static void IsDataProviderValid(SeederConfiguration configuration)
        {
            if (configuration.DataProvider == null)
            {
                throw new MissingConfigurationException("DataProvider");
            }
        }
    }
}