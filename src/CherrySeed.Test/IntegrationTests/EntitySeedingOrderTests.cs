using System.Collections.Generic;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    [TestClass]
    public class EntitySeedingOrderTests
    {
        private CherrySeedDriver _cherrySeedDriver;

        [TestInitialize]
        public void Setup()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void SeedingMultipleEntities()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithSimpleProperties",
                    "MyInteger", "MyString", "MyBool", "MyDateTime", "MyDouble", "MyDecimal")
                    .WithEntity("1", "MyString 1", "true", "2016/05/03", "123,12", "12,33")
                    .WithEntity("2", "MyString 1", "true", "2016/05/03", "123,12", "12,33")
                    .Build(),

                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithNullableProperties",
                    "MyInteger", "MyString", "MyBool", "MyDateTime", "MyDouble", "MyDecimal")
                    .WithEntity("1", "MyString 1", "true", "2016/05/03", "123,12", "12,33")
                    .WithEntity("2", "MyString 1", "true", "2016/05/03", "123,12", "12,33")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithSimpleProperties>();
                cfg.ForEntity<EntityWithNullableProperties>();
            });

            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 4);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithSimpleProperties>(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithNullableProperties>(), 2);
            Assert.IsTrue(repository.GetSeedingDateTime<EntityWithSimpleProperties>().Ticks < repository.GetSeedingDateTime<EntityWithNullableProperties>().Ticks);
        }

        [TestMethod]
        public void ClearingMultipleEntities()
        {
            // Arrange 
            var entityData = new List<EntityData>
            {
                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithSimpleProperties",
                    "MyInteger", "MyString", "MyBool", "MyDateTime", "MyDouble", "MyDecimal")
                    .WithEntity("1", "MyString 1", "true", "2016/05/03", "123,12", "12,33")
                    .WithEntity("2", "MyString 1", "true", "2016/05/03", "123,12", "12,33")
                    .Build(),

                new EntityDataBuilder("CherrySeed.Test.Models.EntityWithNullableProperties",
                    "MyInteger", "MyString", "MyBool", "MyDateTime", "MyDouble", "MyDecimal")
                    .WithEntity("1", "MyString 1", "true", "2016/05/03", "123,12", "12,33")
                    .WithEntity("2", "MyString 1", "true", "2016/05/03", "123,12", "12,33")
                    .Build()
            };

            // Act
            var repository = new InMemoryRepository();
            _cherrySeedDriver.InitAndSeed(entityData.ToDictionaryDataProvider(), repository, cfg =>
            {
                cfg.ForEntity<EntityWithSimpleProperties>();
                cfg.ForEntity<EntityWithNullableProperties>();
            });
            
            // Assert
            Assert.AreEqual(repository.CountSeededObjects(), 4);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithSimpleProperties>(), 2);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithNullableProperties>(), 2);

            _cherrySeedDriver.Clear();

            Assert.AreEqual(repository.CountSeededObjects(), 0);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithSimpleProperties>(), 0);
            Assert.AreEqual(repository.CountSeededObjects<EntityWithNullableProperties>(), 0);

            var entityWithNullablePropertiesClearingTime = repository.GetClearingDateTime<EntityWithNullableProperties>();
            var entityWithSimplePropertiesClearingTime = repository.GetClearingDateTime<EntityWithSimpleProperties>();
            Assert.IsTrue(entityWithNullablePropertiesClearingTime.Ticks < entityWithSimplePropertiesClearingTime.Ticks);
        }
    }
}
