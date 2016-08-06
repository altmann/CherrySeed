using System;
using System.Collections.Generic;
using System.Data.Entity;
using CherrySeed.EntityDefinitions;
using CherrySeed.Repositories;
using CherrySeed.Repositories.Ef;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    public enum Sex
    {
        Male = 3,
        Female = 5
    }

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool IsEnabled { get; set; }
        public int AddressId { get; set; }
        public Sex Sex { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
    }

    public class TestCreateEntityRepository : ICreateRepository, IRemoveRepository
    {
        public void SaveEntity(object obj)
        {
            if (obj is Person)
            {
                var person = (Person)obj;

                Assert.AreEqual("Michael", person.Name);
                Assert.AreEqual(new DateTime(2016, 1, 1), person.Date);
                Assert.AreEqual(true, person.IsEnabled);
                Assert.AreEqual(5, person.AddressId);
                Assert.AreEqual(Sex.Female, person.Sex);

                person.Id = 1;
            }
            if (obj is Address)
            {
                var address = (Address)obj;

                Assert.AreEqual("Street 1", address.Street);
                address.Id = 5;
            }
        }

        public void RemoveEntities(Type type)
        {
            
        }
    }

    [TestClass]
    public class ObjectWithIntIdTests
    {
        [TestMethod]
        public void ObjectWithIntId()
        {
            var okoa = new CherrySeed.CherrySeeder();

            var objectDefinitions = new List<EntityDefinition>
            {
                new EntityDefinition
                {
                    EntityName = "CherrySeed.Test.IntegrationTests.Address",
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
                    EntityName = "CherrySeed.Test.IntegrationTests.Person",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "Id", "1" },
                            { "Name", "Michael" },
                            { "Date", "2016-01-01" },
                            { "IsEnabled", "true" },
                            { "AddressId", "1" },
                            { "Sex", "Female" }
                        }
                    }
                },
            };

            var testTarget = new TestCreateEntityRepository();
            var efTarget = new EfRepository(() => new ApplicationDbContext());

            okoa.EntityDefinitionProvider = new DefaultEntityDefinitionProvider(objectDefinitions);
            okoa.DefaultCreateRepository = efTarget;// testTarget;
            okoa.DefaultRemoveEntitiesRepository = efTarget;// testTarget;

            okoa.AfterTransformation = (oDict, o) => { };

            okoa.InitEntitySettings(cfg =>
            {
                cfg.ForEntity(typeof (Address));

                cfg.ForEntity(typeof (Person))
                    .WithReference(p => ((Person) p).AddressId, typeof (Address));
            });

            okoa.StartSeeding();
        }
    }
}