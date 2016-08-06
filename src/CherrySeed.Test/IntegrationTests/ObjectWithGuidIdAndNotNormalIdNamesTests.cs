using System;
using System.Collections.Generic;
using CherrySeed.EntityDefinitions;
using CherrySeed.Repositories;
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
        public Guid Id { get; set; }
        public string Street { get; set; }
    }

    public class TestCreateEntityTarget3 : ICreateRepository, IRemoveRepository
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
                address.Id = new Guid(1, 2, 3, 4, 5, 6, 1, 1, 1, 1, 1);
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
            var okoa = new CherrySeeder();

            var objectDefinitions = new List<EntityDefinition>
            {
                new EntityDefinition
                {
                    EntityName = "CherrySeed.Test.IntegrationTests.Address3",
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

            var testTarget = new TestCreateEntityTarget3();

            okoa.EntityDefinitionProvider = new DefaultEntityDefinitionProvider(objectDefinitions);
            okoa.DefaultCreateRepository = testTarget;
            okoa.DefaultRemoveEntitiesRepository = testTarget;

            okoa.InitEntitySettings(cfg =>
            {
                cfg.ForEntity(typeof (Address3));
                    
                cfg.ForEntity(typeof(Person3))
                    .WithPrimaryKey<Person3>(e => e.PersonId)
                    .WithReference<Person3>(p => p.AddressId, typeof(Address3));
            });
            
            okoa.StartSeeding();
        }
    }
}