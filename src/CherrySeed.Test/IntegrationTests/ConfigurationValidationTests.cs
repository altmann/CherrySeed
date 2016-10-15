using System.Collections.Generic;
using CherrySeed.Configuration;
using CherrySeed.Configuration.Exceptions;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Base.Asserts;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    [TestClass]
    public class ConfigurationValidationTests
    {
        [TestMethod]
        public void DataProviderNotSet_MissingConfigurationException()
        {
            AssertHelper.TryCatch(tryAction: () =>
            {
                var config = new CherrySeedConfiguration(cfg =>
                {
                    cfg.WithRepository(new EmptyRepository());
                });

                config.CreateSeeder();
            }, catchAction: ex =>
            {
                AssertHelper.AssertExceptionWithMessage(ex, typeof(MissingConfigurationException), "DataProvider");
            });
        }

        [TestMethod]
        public void RepositoryNotSet_MissingConfigurationException()
        {
            AssertHelper.TryCatch(tryAction: () =>
            {
                var config = new CherrySeedConfiguration(cfg =>
                {
                    cfg.WithDataProvider(new DictionaryDataProvider(new List<EntityData>()));
                });

                config.CreateSeeder();
            }, catchAction: ex =>
            {
                AssertHelper.AssertExceptionWithMessage(ex, typeof(MissingConfigurationException), "Repository");
            });
        }
    }
}
