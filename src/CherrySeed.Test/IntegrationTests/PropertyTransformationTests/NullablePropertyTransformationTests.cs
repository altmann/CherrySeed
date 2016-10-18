using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests.PropertyTransformationTests
{
    [TestClass]
    public class NullablePropertyTransformationTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void TransformNullablePropertyTypes()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithNullableProperties",
                    "MyInteger", "MyString", "MyBool", "MyDateTime", "MyDouble", "MyDecimal")
                    .WithEntity("1", "MyString 1", "true", "2016/05/03", "123,12", "12,33")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithNullableProperties>();
            });

            // Assert
            Assert.AreEqual(1, repository.CountSeededObjects());
            Assert.AreEqual(1, repository.CountSeededObjects<EntityWithNullableProperties>());
            EntityAsserts.AssertEntityWithNullableProperties(repository.GetEntities<EntityWithNullableProperties>().First(), new EntityWithNullableProperties
            {
                MyInteger = 1,
                MyString = "MyString 1",
                MyBool = true,
                MyDateTime = new DateTime(2016, 5, 3),
                MyDouble = 123.12,
                MyDecimal = 12.33m
            });
        }

        [TestMethod]
        public void TransformNullablePropertyTypesWithNull()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithNullableProperties",
                    "MyInteger", "MyString", "MyBool", "MyDateTime", "MyDouble", "MyDecimal")
                    .WithEntity("", "", "", "", "", "")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithNullableProperties>();
            });

            // Assert
            Assert.AreEqual(1, repository.CountSeededObjects());
            Assert.AreEqual(1, repository.CountSeededObjects<EntityWithNullableProperties>());
            EntityAsserts.AssertEntityWithNullableProperties(repository.GetEntities<EntityWithNullableProperties>().First(), new EntityWithNullableProperties
            {
                MyInteger = null,
                MyString = null,
                MyBool = null,
                MyDateTime = null,
                MyDouble = null,
                MyDecimal = null
            });
        }
    }
}
