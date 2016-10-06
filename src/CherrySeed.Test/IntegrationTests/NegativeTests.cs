using System.Collections.Generic;
using CherrySeed.Configuration;
using CherrySeed.Configuration.Exceptions;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    [TestClass]
    public class NegativeTests
    {
        private readonly CherrySeedDriver _cherrySeedDriver;

        public NegativeTests()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void DataProviderNotSet_MissingConfigurationException()
        {
            AssertHelper.TryCatch(tryAction: () =>
            {
                var config = new CherrySeedConfiguration(cfg =>
                {
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

        [TestMethod]
        public void IncorrectProperty_PropertyMappingException()
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