using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Convert;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    [TestClass]
    public class EntityReferenceTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void IntegerIdReference()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build(),
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithIntReference",
                    "ReferenceId")
                    .WithEntity("E2")
                    .WithEntity("E2")
                    .Build(),
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<EntityWithConformIntPk>();
                cfg.ForEntity<EntityWithIntReference>()
                    .WithReference(e => e.ReferenceId, typeof(EntityWithConformIntPk));
            });

            // Assert
            Assert.AreEqual(4, repository.CountSeededObjects());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithConformIntPk>());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithIntReference>());
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[0], new EntityWithConformIntPk
            {
                Id = 1
            });
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[1], new EntityWithConformIntPk
            {
                Id = 2
            });

            EntityAsserts.AssertEntityWithIntReference(repository.GetEntities<EntityWithIntReference>()[0], new EntityWithIntReference
            {
                ReferenceId = 2
            });
            EntityAsserts.AssertEntityWithIntReference(repository.GetEntities<EntityWithIntReference>()[1], new EntityWithIntReference
            {
                ReferenceId = 2
            });
        }

        [TestMethod]
        public void GuidIdReference()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformGuidPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build(),
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithGuidReference",
                    "ReferenceId")
                    .WithEntity("E2")
                    .WithEntity("E2")
                    .Build(),
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithCustomPrimaryKeyIdGenerationInApplication(new SequentialGuidPrimaryKeyIdGenerator());

                cfg.ForEntity<EntityWithConformGuidPk>();
                cfg.ForEntity<EntityWithGuidReference>()
                    .WithReference(e => e.ReferenceId, typeof(EntityWithConformGuidPk));
            });

            // Assert
            Assert.AreEqual(4, repository.CountSeededObjects());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithConformGuidPk>());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithGuidReference>());
            EntityAsserts.AssertEntityWithConformGuidPk(repository.GetEntities<EntityWithConformGuidPk>()[0], new EntityWithConformGuidPk
            {
                Id = 1.ToGuid()
            });
            EntityAsserts.AssertEntityWithConformGuidPk(repository.GetEntities<EntityWithConformGuidPk>()[1], new EntityWithConformGuidPk
            {
                Id = 2.ToGuid()
            });

            EntityAsserts.AssertEntityWithGuidReference(repository.GetEntities<EntityWithGuidReference>()[0], new EntityWithGuidReference
            {
                ReferenceId = 2.ToGuid()
            });
            EntityAsserts.AssertEntityWithGuidReference(repository.GetEntities<EntityWithGuidReference>()[1], new EntityWithGuidReference
            {
                ReferenceId = 2.ToGuid()
            });
        }

        [TestMethod]
        public void StringIdReference()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformStringPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build(),
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithStringReference",
                    "ReferenceId")
                    .WithEntity("E2")
                    .WithEntity("E2")
                    .Build(),
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithCustomPrimaryKeyIdGenerationInApplication(new SequentialStringPrimaryKeyIdGenerator(""));

                cfg.ForEntity<EntityWithConformStringPk>();
                cfg.ForEntity<EntityWithStringReference>()
                    .WithReference(e => e.ReferenceId, typeof(EntityWithConformStringPk));
            });

            // Assert
            Assert.AreEqual(4, repository.CountSeededObjects());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithConformStringPk>());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithStringReference>());
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[0], new EntityWithConformStringPk
            {
                Id = "1"
            });
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[1], new EntityWithConformStringPk
            {
                Id = "2"
            });

            EntityAsserts.AssertEntityWithStringReference(repository.GetEntities<EntityWithStringReference>()[0], new EntityWithStringReference
            {
                ReferenceId = "2"
            });
            EntityAsserts.AssertEntityWithStringReference(repository.GetEntities<EntityWithStringReference>()[1], new EntityWithStringReference
            {
                ReferenceId = "2"
            });
        }

        [TestMethod]
        public void StringModelReference()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformStringPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build(),
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithStringReferenceModel",
                    "ReferenceModel")
                    .WithEntity("E2")
                    .WithEntity("E2")
                    .Build(),
            };

            // Act
            var repository = new InMemoryRepository((o, id) =>
            {
                var pk = o as EntityWithConformStringPk;
                if (pk != null)
                    return pk.Id == id;

                throw new InvalidOperationException("Failed");
            });
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithCustomPrimaryKeyIdGenerationInApplication(new SequentialStringPrimaryKeyIdGenerator(""));

                cfg.ForEntity<EntityWithConformStringPk>();
                cfg.ForEntity<EntityWithStringReferenceModel>()
                    .WithReference(e => e.ReferenceModel, typeof(EntityWithConformStringPk));
            });

            // Assert
            Assert.AreEqual(4, repository.CountSeededObjects());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithConformStringPk>());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithStringReferenceModel>());
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[0], new EntityWithConformStringPk
            {
                Id = "1"
            });
            EntityAsserts.AssertEntityWithConformStringPk(repository.GetEntities<EntityWithConformStringPk>()[1], new EntityWithConformStringPk
            {
                Id = "2"
            });

            EntityAsserts.AssertEntityWithStringReferenceModel(repository.GetEntities<EntityWithStringReferenceModel>()[0], new EntityWithStringReferenceModel
            {
                ReferenceModel = new EntityWithConformStringPk { Id = "2" }
            });
            EntityAsserts.AssertEntityWithStringReferenceModel(repository.GetEntities<EntityWithStringReferenceModel>()[1], new EntityWithStringReferenceModel
            {
                ReferenceModel = new EntityWithConformStringPk { Id = "2"}
            });
        }

        [TestMethod]
        public void GuidModelReference()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformGuidPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build(),
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithGuidReferenceModel",
                    "ReferenceModel")
                    .WithEntity("E2")
                    .WithEntity("E2")
                    .Build(),
            };

            // Act
            var repository = new InMemoryRepository((o, id) =>
            {
                var pk = o as EntityWithConformGuidPk;
                if (pk != null)
                    return pk.Id == (Guid)id;

                throw new InvalidOperationException("Failed");
            });
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithCustomPrimaryKeyIdGenerationInApplication(new SequentialGuidPrimaryKeyIdGenerator());

                cfg.ForEntity<EntityWithConformGuidPk>();
                cfg.ForEntity<EntityWithGuidReferenceModel>()
                    .WithReference(e => e.ReferenceModel, typeof(EntityWithConformGuidPk));
            });

            // Assert
            Assert.AreEqual(4, repository.CountSeededObjects());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithConformGuidPk>());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithGuidReferenceModel>());
            EntityAsserts.AssertEntityWithConformGuidPk(repository.GetEntities<EntityWithConformGuidPk>()[0], new EntityWithConformGuidPk
            {
                Id = 1.ToGuid()
            });
            EntityAsserts.AssertEntityWithConformGuidPk(repository.GetEntities<EntityWithConformGuidPk>()[1], new EntityWithConformGuidPk
            {
                Id = 2.ToGuid()
            });

            EntityAsserts.AssertEntityWithGuidReferenceModel(repository.GetEntities<EntityWithGuidReferenceModel>()[0], new EntityWithGuidReferenceModel
            {
                ReferenceModel = new EntityWithConformGuidPk { Id = 2.ToGuid() }
            });
            EntityAsserts.AssertEntityWithGuidReferenceModel(repository.GetEntities<EntityWithGuidReferenceModel>()[1], new EntityWithGuidReferenceModel
            {
                ReferenceModel = new EntityWithConformGuidPk { Id = 2.ToGuid() }
            });
        }

        [TestMethod]
        public void IntegerModelReference()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithConformIntPk",
                    "Id")
                    .WithEntity("E1")
                    .WithEntity("E2")
                    .Build(),
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithIntReferenceModel",
                    "ReferenceModel")
                    .WithEntity("E2")
                    .WithEntity("E2")
                    .Build(),
            };

            // Act
            var repository = new InMemoryRepository((o, id) =>
            {
                var pk = o as EntityWithConformIntPk;
                if (pk != null)
                    return pk.Id == (int)id;

                throw new InvalidOperationException("Failed");
            });
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<EntityWithConformIntPk>();
                cfg.ForEntity<EntityWithIntReferenceModel>()
                    .WithReference(e => e.ReferenceModel, typeof(EntityWithConformIntPk));
            });

            // Assert
            Assert.AreEqual(4, repository.CountSeededObjects());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithConformIntPk>());
            Assert.AreEqual(2, repository.CountSeededObjects<EntityWithIntReferenceModel>());
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[0], new EntityWithConformIntPk
            {
                Id = 1
            });
            EntityAsserts.AssertEntityWithConformIntPk(repository.GetEntities<EntityWithConformIntPk>()[1], new EntityWithConformIntPk
            {
                Id = 2
            });

            EntityAsserts.AssertEntityWithIntReferenceModel(repository.GetEntities<EntityWithIntReferenceModel>()[0], new EntityWithIntReferenceModel
            {
                ReferenceModel = new EntityWithConformIntPk { Id = 2 }
            });
            EntityAsserts.AssertEntityWithIntReferenceModel(repository.GetEntities<EntityWithIntReferenceModel>()[1], new EntityWithIntReferenceModel
            {
                ReferenceModel = new EntityWithConformIntPk { Id = 2 }
            });
        }
    }
}
