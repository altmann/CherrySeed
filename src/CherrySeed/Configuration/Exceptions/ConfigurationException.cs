using System;

namespace CherrySeed.Configuration.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    public class MissingConfigurationException : ConfigurationException
    {
        public MissingConfigurationException(string configurationKey)
            : base(string.Format("Configuration '{0}' is missing. Set with class CherrySeedConfiguration the required settings.", configurationKey), null)
        {
            
        }
    }
}