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
    public class EnumPropertyTransformationTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void TransformEnumTypes()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithEnumProperty",
                    "EnumProperty1", "EnumProperty2")
                    .WithEntity("EnumValue1", "EnumValue2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithEnumProperty>();
            });

            // Assert
            Assert.AreEqual(1, repository.CountSeededObjects());
            Assert.AreEqual(1, repository.CountSeededObjects<EntityWithEnumProperty>());
            EntityAsserts.AssertEntityWithEnumProperty(repository.GetEntities<EntityWithEnumProperty>().First(), new EntityWithEnumProperty
            {
                EnumProperty1 = TestEnum.EnumValue1,
                EnumProperty2 = TestEnum.EnumValue2
            });
        }

        [TestMethod]
        public void TransformEnumTypesWithNumber()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithEnumProperty",
                    "EnumProperty1", "EnumProperty2")
                    .WithEntity("1", "0")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithEnumProperty>();
            });

            // Assert
            Assert.AreEqual(1, repository.CountSeededObjects());
            Assert.AreEqual(1, repository.CountSeededObjects<EntityWithEnumProperty>());
            EntityAsserts.AssertEntityWithEnumProperty(repository.GetEntities<EntityWithEnumProperty>().First(), new EntityWithEnumProperty
            {
                EnumProperty1 = TestEnum.EnumValue2,
                EnumProperty2 = TestEnum.EnumValue1
            });
        }
    }
}
