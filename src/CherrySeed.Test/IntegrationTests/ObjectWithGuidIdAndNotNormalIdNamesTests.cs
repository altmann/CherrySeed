﻿using System;
using System.Collections.Generic;
using CherrySeed.EntityDataProvider;
using CherrySeed.Repositories;
using CherrySeed.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    public class Person3
    {
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool IsEnabled { get; set; }
        public Guid AddressId { get; set; }
    }

    public class Address3
    {
        public Guid Address3Id { get; set; }
        public string Street { get; set; }
    }

    public class TestEntityTarget3 : IRepository
    {
        public void SaveEntity(object obj)
        {
            if (obj is Person3)
            {
                var person = (Person3)obj;

                Assert.AreEqual("Michael", person.Name);
                Assert.AreEqual(new DateTime(2016, 1, 1), person.Date);
                Assert.AreEqual(true, person.IsEnabled);
                Assert.AreEqual(new Guid(1, 2, 3, 4, 5, 6, 1, 1, 1, 1, 1), person.AddressId);

                person.PersonId = new Guid(1, 2, 3, 4, 5, 6, 1, 1, 1, 1, 1);
            }
            if (obj is Address3)
            {
                var address = (Address3)obj;

                Assert.AreEqual("Street 1", address.Street);
                address.Address3Id = new Guid(1, 2, 3, 4, 5, 6, 1, 1, 1, 1, 1);
            }
        }

        public void RemoveEntities(Type type)
        {
            
        }
    }


    [TestClass]
    public class ObjectWithGuidIdAndNotNormalIdNamesTests
    {
        [TestMethod]
        public void ObjectWithGuidIdAndNotNormalIdNames()
        {
            var cherrySeeder = new CherrySeeder();

            var entityData = new List<EntityDataProvider.EntityData>
            {
                new EntityDataProvider.EntityData
                {
                    EntityName = "CherrySeed.Test.IntegrationTests.Address3",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "Address3Id", "1" },
                            { "Street", "Street 1" },
                        }
                    }
                },
                new EntityDataProvider.EntityData
                {
                    EntityName = "CherrySeed.Test.IntegrationTests.Person3",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "PersonId", "1" },
                            { "Name", "Michael" },
                            { "Date", "2016-01-01" },
                            { "IsEnabled", "true" },
                            { "AddressId", "1" }
                        }
                    }
                },
            };

            var testTarget = new TestEntityTarget3();

            cherrySeeder.UseDictionaryDataProvider(entityData);
            cherrySeeder.DefaultRepository = testTarget;

            cherrySeeder.InitEntitySettings(cfg =>
            {
                cfg.ForEntity<Address3>();
                    
                cfg.ForEntity<Person3>()
                    .WithPrimaryKey(e => e.PersonId)
                    .WithReference(p => p.AddressId, typeof(Address3));
            });
            
            cherrySeeder.Seed();
        }
    }
}