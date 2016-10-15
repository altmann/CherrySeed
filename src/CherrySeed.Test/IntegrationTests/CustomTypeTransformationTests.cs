using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    [TestClass]
    public class CustomTypeTransformationTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void TransformPropertyWithOverridenTypeTransformation()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithSimpleProperties",
                    "MyString")
                    .WithEntity("This is a sample text")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.AddTypeTransformation(typeof(string), new CustomTypeTransformation<string>("New sample text"));
                cfg.ForEntity<EntityWithSimpleProperties>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithSimpleProperties>(), 1);
            EntityAsserts.AssertEntityWithSimpleProperties(repository.GetEntities<EntityWithSimpleProperties>().First(), new EntityWithSimpleProperties
            {
                MyString = "New sample text"
            });
        }

        [TestMethod]
        public void TransformPropertyWithNotSupportedTypeTransformation()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithNotSupportedTypeProperty",
                    "UintProperty")
                    .WithEntity("12345")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.AddTypeTransformation(typeof(uint), new CustomTypeTransformation<uint>(123));
                cfg.ForEntity<EntityWithNotSupportedTypeProperty>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithNotSupportedTypeProperty>(), 1);
            EntityAsserts.AssertEntityWithNotSupportedProperty(repository.GetEntities<EntityWithNotSupportedTypeProperty>().First(), new EntityWithNotSupportedTypeProperty
            {
                UintProperty = 123
            });
        }
    }
}
