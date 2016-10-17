using System.Collections.Generic;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests.PrimaryKeyTests
{
    [TestClass]
    public class PrimaryKeyIdGenerationTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void PrimaryKeyIdGenerationInDatabase()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithConformIntPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformIntPk>(), 2);
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[0], new EntityWithConformIntPk
            {
                Id = 0
            });
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[1], new EntityWithConformIntPk
            {
                Id = 0
            });
        }

        [TestMethod]
        public void PrimaryKeyIdGenerationInApplicationAsInteger()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
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
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformIntPk>(), 2);
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[0], new EntityWithConformIntPk
            {
                Id = 1
            });
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[1], new EntityWithConformIntPk
            {
                Id = 2
            });
        }

        [TestMethod]
        public void DisablePrimaryKeyIdGeneration()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk",
                    "Id")
                    .WithEntity("10")
                    .WithEntity("20")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.DisablePrimaryKeyIdGeneration();

                cfg.ForEntity<EntityWithConformIntPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformIntPk>(), 2);
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[0], new EntityWithConformIntPk
            {
                Id = 10
            });
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[1], new EntityWithConformIntPk
            {
                Id = 20
            });
        }

        [TestMethod]
        public void PrimaryKeyIdGenerationInApplicationAsInteger_StartWith100()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsInteger(100);

                cfg.ForEntity<EntityWithConformIntPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformIntPk>(), 2);
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[0], new EntityWithConformIntPk
            {
                Id = 100
            });
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[1], new EntityWithConformIntPk
            {
                Id = 101
            });
        }

        [TestMethod]
        public void PrimaryKeyIdGenerationInApplicationAsInteger_Step20()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsInteger(1, 20);

                cfg.ForEntity<EntityWithConformIntPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformIntPk>(), 2);
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[0], new EntityWithConformIntPk
            {
                Id = 1
            });
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[1], new EntityWithConformIntPk
            {
                Id = 21
            });
        }

        [TestMethod]
        public void PrimaryKeyIdGenerationInApplicationAsGuid()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformGuidPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsGuid();

                cfg.ForEntity<EntityWithConformGuidPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformGuidPk>(), 2);
            EntityAsserts.AssertEntityWithConformGuidPk(repository.GetEntities<EntityWithConformGuidPk>()[0]);
            EntityAsserts.AssertEntityWithConformGuidPk(repository.GetEntities<EntityWithConformGuidPk>()[1]);
        }

        [TestMethod]
        public void PrimaryKeyIdGenerationInApplicationAsString()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformStringPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsString();

                cfg.ForEntity<EntityWithConformStringPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformStringPk>(), 2);
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[0], new EntityWithConformStringPk
            {
                Id = "1"
            });
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[1], new EntityWithConformStringPk
            {
                Id = "2"
            });
        }

        [TestMethod]
        public void PrimaryKeyIdGenerationInApplicationAsString_StartWith100()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformStringPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsString("", 100);

                cfg.ForEntity<EntityWithConformStringPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformStringPk>(), 2);
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[0], new EntityWithConformStringPk
            {
                Id = "100"
            });
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[1], new EntityWithConformStringPk
            {
                Id = "101"
            });
        }

        [TestMethod]
        public void PrimaryKeyIdGenerationInApplicationAsString_Step20()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformStringPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsString("", 1, 20);

                cfg.ForEntity<EntityWithConformStringPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformStringPk>(), 2);
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[0], new EntityWithConformStringPk
            {
                Id = "1"
            });
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[1], new EntityWithConformStringPk
            {
                Id = "21"
            });
        }

        [TestMethod]
        public void PrimaryKeyIdGenerationInApplicationAsString_WithPrefix()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformStringPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsString("Entity");

                cfg.ForEntity<EntityWithConformStringPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformStringPk>(), 2);
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[0], new EntityWithConformStringPk
            {
                Id = "Entity1"
            });
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[1], new EntityWithConformStringPk
            {
                Id = "Entity2"
            });
        }

        [TestMethod]
        public void CustomPrimaryKeyIdGeneration()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformStringPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithCustomPrimaryKeyIdGenerationInApplication(new SequentialStringPrimaryKeyIdGenerator("CUSTOM"));

                cfg.ForEntity<EntityWithConformStringPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformStringPk>(), 2);
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[0], new EntityWithConformStringPk
            {
                Id = "CUSTOM1"
            });
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[1], new EntityWithConformStringPk
            {
                Id = "CUSTOM2"
            });
        }

        [TestMethod]
        public void DisabledPrimaryKeyIdGeneration()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk",
                    "Id")
                    .WithEntity("1")
                    .WithEntity("2")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.DisablePrimaryKeyIdGeneration();

                cfg.ForEntity<EntityWithConformIntPk>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithConformIntPk>(), 2);
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[0], new EntityWithConformIntPk
            {
                Id = 1
            });
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[1], new EntityWithConformIntPk
            {
                Id = 2
            });
        }
    }
}
