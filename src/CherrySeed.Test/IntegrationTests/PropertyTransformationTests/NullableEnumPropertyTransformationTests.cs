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
    public class NullableEnumPropertyTransformationTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void TransformNullableEnumTypes()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithNullableEnumProperty",
                    "EnumProperty1", "EnumProperty2")
                    .WithEntity("EnumValue1", "EnumValue2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithNullableEnumProperty>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithNullableEnumProperty>(), 1);
            EntityAsserts.AssertEntityWithNullableEnumProperty(repository.GetEntities<EntityWithNullableEnumProperty>().First(), new EntityWithNullableEnumProperty
            {
                EnumProperty1 = TestEnum.EnumValue1,
                EnumProperty2 = TestEnum.EnumValue2
            });
        }

        [TestMethod]
        public void TransformNullableEnumTypesWithNull()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithNullableEnumProperty",
                    "EnumProperty1", "EnumProperty2")
                    .WithEntity("", "")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithNullableEnumProperty>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithNullableEnumProperty>(), 1);
            EntityAsserts.AssertEntityWithNullableEnumProperty(repository.GetEntities<EntityWithNullableEnumProperty>().First(), new EntityWithNullableEnumProperty
            {
                EnumProperty1 = null,
                EnumProperty2 = null
            });
        }

        [TestMethod]
        public void TransformNullableEnumTypesWithNumbers()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithNullableEnumProperty",
                    "EnumProperty1", "EnumProperty2")
                    .WithEntity("0", "1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithNullableEnumProperty>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithNullableEnumProperty>(), 1);
            EntityAsserts.AssertEntityWithNullableEnumProperty(repository.GetEntities<EntityWithNullableEnumProperty>().First(), new EntityWithNullableEnumProperty
            {
                EnumProperty1 = TestEnum.EnumValue1,
                EnumProperty2 = TestEnum.EnumValue2
            });
        }
    }
}
