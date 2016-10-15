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
    public class EntityNameMatchingTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void SetEntityNameAsFullNameInDataProvider()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithSimpleProperties",
                    "MyInteger")
                    .WithEntity("1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndExecute(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithSimpleProperties>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithSimpleProperties>(), 1);
            EntityAsserts.AssertEntityWithSimpleProperties(repository.GetEntities<EntityWithSimpleProperties>().First(), new EntityWithSimpleProperties
            {
                MyInteger = 1
            });
        }

        [TestMethod]
        public void SetEntityNameAsClassNameInDataProvider()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("EntityWithSimpleProperties",
                    "MyInteger")
                    .WithEntity("1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndExecute(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithSimpleProperties>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithSimpleProperties>(), 1);
            EntityAsserts.AssertEntityWithSimpleProperties(repository.GetEntities<EntityWithSimpleProperties>().First(), new EntityWithSimpleProperties
            {
                MyInteger = 1
            });
        }

        [TestMethod]
        public void SetEntityNameAsCustomNameInDataProvider()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("MyEntity",
                    "MyInteger")
                    .WithEntity("1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndExecute(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithSimpleProperties>()
                    .HasEntityName("MyEntity");
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithSimpleProperties>(), 1);
            EntityAsserts.AssertEntityWithSimpleProperties(repository.GetEntities<EntityWithSimpleProperties>().First(), new EntityWithSimpleProperties
            {
                MyInteger = 1
            });
        }

        [TestMethod]
        public void SetEntityNameAsCustomNameInDataProvider_NoDataInDataProvider()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("OtherEntity",
                    "MyInteger")
                    .WithEntity("1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndExecute(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithSimpleProperties>()
                    .HasEntityName("MyEntity");
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 0);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithSimpleProperties>(), 0);
        }
    }
}
