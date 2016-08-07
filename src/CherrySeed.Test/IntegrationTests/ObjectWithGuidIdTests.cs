using System;
using System.Collections.Generic;
using CherrySeed.EntityDefinitions;
using CherrySeed.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    public class Person2
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool IsEnabled { get; set; }
        public Guid AddressId { get; set; }
    }

    public class Address2
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
    }

    public class TestCreateEntityTarget2 : ICreateRepository, IRemoveRepository
    {
        public void SaveEntity(object obj)
        {
            if (obj is Person2)
            {
                var person = (Person2)obj;

                Assert.AreEqual("Michael", person.Name);
                Assert.AreEqual(new DateTime(2016, 1, 1), person.Date);
                Assert.AreEqual(true, person.IsEnabled);
                Assert.AreEqual(new Guid(1, 2, 3, 4, 5, 6, 1, 1, 1, 1, 1), person.AddressId);

                person.Id = new Guid(1, 2, 3, 4, 5, 6, 1, 1, 1, 1, 1);
            }
            if (obj is Address2)
            {
                var address = (Address2)obj;

                Assert.AreEqual("Street 1", address.Street);
                address.Id = new Guid(1, 2, 3, 4, 5, 6, 1, 1, 1, 1, 1);
            }
        }

        public void RemoveEntities(Type type)
        {
            
        }
    }

    [TestClass]
    public class ObjectWithGuidIdTest
    {
        [TestMethod]
        public void ObjectWithGuidId()
        {
            var cherrySeeder = new CherrySeed.CherrySeeder();

            var objectDefinitions = new List<EntityDefinition>
            {
                new EntityDefinition
                {
                    EntityName = "CherrySeed.Test.IntegrationTests.Address2",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "Id", "1" },
                            { "Street", "Street 1" },
                        }
                    }
                },
                new EntityDefinition
                {
                    EntityName = "CherrySeed.Test.IntegrationTests.Person2",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "Id", "1" },
                            { "Name", "Michael" },
                            { "Date", "2016-01-01" },
                            { "IsEnabled", "true" },
                            { "AddressId", "1" }
                        }
                    }
                },
            };

            var testTarget = new TestCreateEntityTarget2();

            cherrySeeder.EntityDefinitionProvider = new DictionaryDataProvider(objectDefinitions);
            cherrySeeder.DefaultCreateRepository = testTarget;
            cherrySeeder.DefaultRemoveRepository = testTarget;

            cherrySeeder.InitEntitySettings(cfg =>
            {
                cfg.ForEntity<Address2>();

                cfg.ForEntity<Person2>()
                    .WithReference(p => p.AddressId, typeof(Address2));
            });

            cherrySeeder.Seed();
        }
    }
}