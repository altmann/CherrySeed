using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    [TestClass]
    public class DefaultValuePropertyTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void EntityPropertiesWithDefaultValues()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithSimpleProperties")
                    .WithEntity()
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithSimpleProperties>()
                    .WithFieldWithDefaultValue(e => e.MyBool, () => true)
                    .WithFieldWithDefaultValue(e => e.MyDateTime, () => new DateTime(2016, 10, 15))
                    .WithFieldWithDefaultValue(e => e.MyDecimal, () => 12.12m)
                    .WithFieldWithDefaultValue(e => e.MyDouble, () => 123.123)
                    .WithFieldWithDefaultValue(e => e.MyInteger, () => 9988)
                    .WithFieldWithDefaultValue(e => e.MyString, () => "My default string");
            });

            // Assert
            Assert.AreEqual(1, repository.CountSeededObjects());
            Assert.AreEqual(1, repository.CountSeededObjects<EntityWithSimpleProperties>());
            EntityAsserts.AssertEntityWithSimpleProperties(repository.GetEntities<EntityWithSimpleProperties>().First(), new EntityWithSimpleProperties
            {
                MyInteger = 9988,
                MyString = "My default string",
                MyBool = true,
                MyDateTime = new DateTime(2016, 10, 15),
                MyDouble = 123.123,
                MyDecimal = 12.12m
            });
        }

        [TestMethod]
        public void EntityPropertiesWithDefaultValues_OverrideDefaultValuesInDataProvider()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithSimpleProperties", "MyBool", "MyString")
                    .WithEntity("false", "My string from data provider")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithSimpleProperties>()
                    .WithFieldWithDefaultValue(e => e.MyBool, () => true)
                    .WithFieldWithDefaultValue(e => e.MyDateTime, () => new DateTime(2016, 10, 15))
                    .WithFieldWithDefaultValue(e => e.MyDecimal, () => 12.12m)
                    .WithFieldWithDefaultValue(e => e.MyDouble, () => 123.123)
                    .WithFieldWithDefaultValue(e => e.MyInteger, () => 9988)
                    .WithFieldWithDefaultValue(e => e.MyString, () => "My default string");
            });

            // Assert
            Assert.AreEqual(1, repository.CountSeededObjects());
            Assert.AreEqual(1, repository.CountSeededObjects<EntityWithSimpleProperties>());
            EntityAsserts.AssertEntityWithSimpleProperties(repository.GetEntities<EntityWithSimpleProperties>().First(), new EntityWithSimpleProperties
            {
                MyInteger = 9988,
                MyString = "My string from data provider",
                MyBool = false,
                MyDateTime = new DateTime(2016, 10, 15),
                MyDouble = 123.123,
                MyDecimal = 12.12m
            });
        }
    }
}
