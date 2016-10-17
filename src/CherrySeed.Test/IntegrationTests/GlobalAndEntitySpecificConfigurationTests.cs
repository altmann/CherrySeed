using CherrySeed.Configuration;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    [TestClass]
    public class GlobalAndEntitySpecificConfigurationTests
    {
        private SeederConfigurationBuilder _configBuilder;

        [TestInitialize]
        public void Setup()
        {
            _configBuilder = new SeederConfigurationBuilder();

            // setting data provider is necessary because of config validation
            _configBuilder.WithDataProvider(new DictionaryDataProvider(null));

            // setting repository is necessary because of config validation
            var globalRepository = new InMemoryRepository();
            _configBuilder.WithRepository(globalRepository);
        }

        [TestMethod]
        public void SetGlobalAndEntitySpecificRepository()
        {
            // Arrange global settings
            var globalRepository = new InMemoryRepository();
            _configBuilder.WithRepository(globalRepository);

            // Arrange entity specific settings
            _configBuilder.ForEntity<EntityWithSimpleProperties>();

            var entitySpecificRepository = new InMemoryRepository();
            _configBuilder.ForEntity<EntityWithConformIntPk>()
                .WithRepository(entitySpecificRepository);

            // Act
            var config = _configBuilder.Build();

            // Assert
            Assert.AreEqual(globalRepository, config.EntitySettings.GetSetting<EntityWithSimpleProperties>().Repository);
            Assert.AreEqual(entitySpecificRepository, config.EntitySettings.GetSetting<EntityWithConformIntPk>().Repository);
        }

        [TestMethod]
        public void DisableGlobalAndSetEntitySpecificIntegerPrimaryKeyIdGeneration()
        {
            // Arrange global settings
            _configBuilder.DisablePrimaryKeyIdGeneration();

            // Arrange entity specific settings
            _configBuilder.ForEntity<EntityWithSimpleProperties>();

            _configBuilder.ForEntity<EntityWithConformIntPk>()
                .WithPrimaryKeyIdGenerationInApplicationAsInteger();

            // Act
            var config = _configBuilder.Build();

            // Assert
            var entitySetting1 = config.EntitySettings.GetSetting<EntityWithSimpleProperties>();
            Assert.IsFalse(entitySetting1.IdGeneration.IsGeneratorEnabled);
            Assert.IsNull(entitySetting1.IdGeneration.Generator);

            var entitySetting2 = config.EntitySettings.GetSetting<EntityWithConformIntPk>();
            Assert.IsTrue(entitySetting2.IdGeneration.IsGeneratorEnabled);
            Assert.IsNotNull(entitySetting2.IdGeneration.Generator);
        }

        [TestMethod]
        public void DisableGlobalAndSetEntitySpecificGuidPrimaryKeyIdGeneration()
        {
            // Arrange global settings
            _configBuilder.DisablePrimaryKeyIdGeneration();

            // Arrange entity specific settings
            _configBuilder.ForEntity<EntityWithSimpleProperties>();

            _configBuilder.ForEntity<EntityWithConformIntPk>()
                .WithPrimaryKeyIdGenerationInApplicationAsGuid();

            // Act
            var config = _configBuilder.Build();

            // Assert
            var entitySetting1 = config.EntitySettings.GetSetting<EntityWithSimpleProperties>();
            Assert.IsFalse(entitySetting1.IdGeneration.IsGeneratorEnabled);
            Assert.IsNull(entitySetting1.IdGeneration.Generator);

            var entitySetting2 = config.EntitySettings.GetSetting<EntityWithConformIntPk>();
            Assert.IsTrue(entitySetting2.IdGeneration.IsGeneratorEnabled);
            Assert.IsNotNull(entitySetting2.IdGeneration.Generator);
        }

        [TestMethod]
        public void DisableGlobalAndSetEntitySpecificStringPrimaryKeyIdGeneration()
        {
            // Arrange global settings
            _configBuilder.DisablePrimaryKeyIdGeneration();

            // Arrange entity specific settings
            _configBuilder.ForEntity<EntityWithSimpleProperties>();

            _configBuilder.ForEntity<EntityWithConformIntPk>()
                .WithPrimaryKeyIdGenerationInApplicationAsString();

            // Act
            var config = _configBuilder.Build();

            // Assert
            var entitySetting1 = config.EntitySettings.GetSetting<EntityWithSimpleProperties>();
            Assert.IsFalse(entitySetting1.IdGeneration.IsGeneratorEnabled);
            Assert.IsNull(entitySetting1.IdGeneration.Generator);

            var entitySetting2 = config.EntitySettings.GetSetting<EntityWithConformIntPk>();
            Assert.IsTrue(entitySetting2.IdGeneration.IsGeneratorEnabled);
            Assert.IsNotNull(entitySetting2.IdGeneration.Generator);
        }

        [TestMethod]
        public void SetGlobalStringPrimaryKeyIdGenerationAndDisableEntitySpecific()
        {
            // Arrange global settings
            _configBuilder.WithPrimaryKeyIdGenerationInApplicationAsString();

            // Arrange entity specific settings
            _configBuilder.ForEntity<EntityWithSimpleProperties>()
                .WithDisabledPrimaryKeyIdGeneration();

            _configBuilder.ForEntity<EntityWithConformIntPk>();

            // Act
            var config = _configBuilder.Build();

            // Assert
            var entitySetting1 = config.EntitySettings.GetSetting<EntityWithSimpleProperties>();
            Assert.IsFalse(entitySetting1.IdGeneration.IsGeneratorEnabled);
            Assert.IsNull(entitySetting1.IdGeneration.Generator);

            var entitySetting2 = config.EntitySettings.GetSetting<EntityWithConformIntPk>();
            Assert.IsTrue(entitySetting2.IdGeneration.IsGeneratorEnabled);
            Assert.IsNotNull(entitySetting2.IdGeneration.Generator);
        }

        [TestMethod]
        public void SetGlobalIntegerPrimaryKeyIdGenerationAndDisableEntitySpecific()
        {
            // Arrange global settings
            _configBuilder.WithPrimaryKeyIdGenerationInApplicationAsInteger();

            // Arrange entity specific settings
            _configBuilder.ForEntity<EntityWithSimpleProperties>()
                .WithDisabledPrimaryKeyIdGeneration();

            _configBuilder.ForEntity<EntityWithConformIntPk>();

            // Act
            var config = _configBuilder.Build();

            // Assert
            var entitySetting1 = config.EntitySettings.GetSetting<EntityWithSimpleProperties>();
            Assert.IsFalse(entitySetting1.IdGeneration.IsGeneratorEnabled);
            Assert.IsNull(entitySetting1.IdGeneration.Generator);

            var entitySetting2 = config.EntitySettings.GetSetting<EntityWithConformIntPk>();
            Assert.IsTrue(entitySetting2.IdGeneration.IsGeneratorEnabled);
            Assert.IsNotNull(entitySetting2.IdGeneration.Generator);
        }

        [TestMethod]
        public void SetGlobalGuidPrimaryKeyIdGenerationAndDisableEntitySpecific()
        {
            // Arrange global settings
            _configBuilder.WithPrimaryKeyIdGenerationInApplicationAsGuid();

            // Arrange entity specific settings
            _configBuilder.ForEntity<EntityWithSimpleProperties>()
                .WithDisabledPrimaryKeyIdGeneration();

            _configBuilder.ForEntity<EntityWithConformIntPk>();

            // Act
            var config = _configBuilder.Build();

            // Assert
            var entitySetting1 = config.EntitySettings.GetSetting<EntityWithSimpleProperties>();
            Assert.IsFalse(entitySetting1.IdGeneration.IsGeneratorEnabled);
            Assert.IsNull(entitySetting1.IdGeneration.Generator);

            var entitySetting2 = config.EntitySettings.GetSetting<EntityWithConformIntPk>();
            Assert.IsTrue(entitySetting2.IdGeneration.IsGeneratorEnabled);
            Assert.IsNotNull(entitySetting2.IdGeneration.Generator);
        }
    }
}
