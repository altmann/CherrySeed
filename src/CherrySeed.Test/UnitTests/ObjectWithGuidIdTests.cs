using System;
using System.Collections.Generic;
using CherrySeed.Configuration;
using CherrySeed.EntityDataProvider;
using CherrySeed.Repositories;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Convert;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.UnitTests
{
    [TestClass]
    public class ObjectWithGuidIdTests
    {
        [TestMethod]
        public void ObjectWithGuidId()
        {
            var entityData = new List<EntityData>
            {
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.Person",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "PersonId", "P1" },
                            { "AddressId", "A1" }
                        },
                        new Dictionary<string, string>
                        {
                            { "PersonId", "P2" },
                            { "AddressId", "A1" }
                        }
                    }
                },
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.Address",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "AddressId", "A1" }
                        },
                        new Dictionary<string, string>
                        {
                            { "AddressId", "A2" }
                        }
                    }
                },
            };

            var assertRepository = new AssertRepository((obj, count) =>
            {
                AssertHelper.AssertIf(typeof(Person), 0, count, obj, () =>
                {
                    AssertPerson.AssertProperties(obj, Converter.ToGuid(1));
                });

                AssertHelper.AssertIf(typeof(Person), 1, count, obj, () =>
                {
                    AssertPerson.AssertProperties(obj, Converter.ToGuid(1));
                });

            }, type =>
            {
                
            });
            
            InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.ForEntity<Address>()
                    .WithCustomIdGenerationViaCode(new SequentialGuidIdGenerator());

                cfg.ForEntity<Person>()
                    .WithReference(e => e.AddressId, typeof(Address))
                    .WithCustomIdGenerationViaCode(new SequentialGuidIdGenerator());
            });
        }

        private void InitAndExecute(List<EntityData> data, IRepository repository, 
            Action<ISeederConfigurationBuilder> entitySettings)
        {
            var config = new SeederConfiguration(cfg =>
            {
                cfg.WithDataProvider(new DictionaryDataProvider(data));
                cfg.WithDefaultRepository(repository);

                entitySettings(cfg);
            });

            var cherrySeeder = config.CreateSeeder();
            cherrySeeder.Seed();
        }
    }
}