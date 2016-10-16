using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests.PrimaryKeyTests
{
    [TestClass]
    public class PrimaryKeyDeclarationTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void Convention_PrimaryKeyWithNameId()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk",
                    "Id")
                    .WithEntity("E1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<EntityWithConformIntPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformIntPk>(), 1);
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>().First(), new EntityWithConformIntPk
            {
                Id = 1
            });
        }

        [TestMethod]
        public void Convention_PrimaryKeyWithNameID()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk2",
                    "ID")
                    .WithEntity("E1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<EntityWithConformIntPk2>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformIntPk2>(), 1);
            EntityAsserts.AssertEntityWithConformIntPk2(repository.GetEntities<EntityWithConformIntPk2>().First(), new EntityWithConformIntPk2
            {
                ID = 1
            });
        }

        [TestMethod]
        public void Convention_PrimaryKeyWithNameClassNameAndId()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk3",
                    "EntityWithConformIntPk3Id")
                    .WithEntity("E1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<EntityWithConformIntPk3>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformIntPk3>(), 1);
            EntityAsserts.AssertEntityWithConformIntPk3(repository.GetEntities<EntityWithConformIntPk3>().First(), new EntityWithConformIntPk3
            {
                EntityWithConformIntPk3Id = 1
            });
        }

        [TestMethod]
        public void Convention_PrimaryKeyWithNameClassNameAndID()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk3",
                    "EntityWithConformIntPk3Id")
                    .WithEntity("E1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<EntityWithConformIntPk3>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformIntPk3>(), 1);
            EntityAsserts.AssertEntityWithConformIntPk3(repository.GetEntities<EntityWithConformIntPk3>().First(), new EntityWithConformIntPk3
            {
                EntityWithConformIntPk3Id = 1
            });
        }

        [TestMethod]
        public void Convention_CustomPrimaryKeyConvention()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithUnconformIntPk",
                    "CustomId")
                    .WithEntity("E1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsInteger();
                cfg.WithDefaultPrimaryKeyNames("CustomId");

                cfg.ForEntity<EntityWithUnconformIntPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithUnconformIntPk>(), 1);
            EntityAsserts.AssertEntityWithUnconformIntPk(repository.GetEntities<EntityWithUnconformIntPk>().First(), new EntityWithUnconformIntPk
            {
                CustomId = 1
            });
        }

        [TestMethod]
        public void NoConvention_CustomPrimaryKey()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithUnconformIntPk",
                    "CustomId")
                    .WithEntity("E1")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<EntityWithUnconformIntPk>()
                    .WithPrimaryKey(e => e.CustomId);
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 1);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithUnconformIntPk>(), 1);
            EntityAsserts.AssertEntityWithUnconformIntPk(repository.GetEntities<EntityWithUnconformIntPk>().First(), new EntityWithUnconformIntPk
            {
                CustomId = 1
            });
        }
    }
}
