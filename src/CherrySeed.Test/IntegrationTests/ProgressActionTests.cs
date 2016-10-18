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
    public class ProgressActionTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void SetGlobalAndEntitySpecificActions()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithSimpleProperties",
                    "MyInteger", "MyString")
                    .WithEntity("1", "MyString 1")
                    .Build()
            };

            var globalBeforeSaveCallCounter = 0;
            var globalAfterSaveCallCounter = 0;
            var entityAfterSaveCallCounter = 0;

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.BeforeSave((dictionary, o) =>
                {
                    globalBeforeSaveCallCounter++;
                });

                cfg.AfterSave(((dictionary, o) =>
                {
                    globalAfterSaveCallCounter++;
                }));

                cfg.ForEntity<EntityWithSimpleProperties>()
                    .AfterSave((o =>
                    {
                        entityAfterSaveCallCounter++;
                    }));
            });

            // Assert
            Assert.AreEqual(1, globalBeforeSaveCallCounter);
            Assert.AreEqual(1, globalAfterSaveCallCounter);
            Assert.AreEqual(1, entityAfterSaveCallCounter);
        }
    }
}