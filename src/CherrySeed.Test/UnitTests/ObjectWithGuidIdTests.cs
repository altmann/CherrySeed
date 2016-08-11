using System;
using System.Collections.Generic;
using CherrySeed.EntityDataProvider;
using CherrySeed.EntitySettings;
using CherrySeed.Repositories;
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
                
            }, type =>
            {
                
            });
            
            InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.ForEntity<Address>()
                    .WithCodeIdGeneration().WithGuidIdGenerator();

                cfg.ForEntity<Person>()
                    .WithReference(e => e.AddressId, typeof(Address))
                    .WithCodeIdGeneration().WithGuidIdGenerator();
            });
        }

        private void InitAndExecute(List<EntityData> data, IRepository repository, 
            Action<CompositeEntitySettingBuilder> entitySettings)
        {
            var cherrySeeder = new CherrySeeder();
            cherrySeeder.UseDictionaryDataProvider(data);
            cherrySeeder.DefaultRepository = repository;

            cherrySeeder.InitEntitySettings(entitySettings);

            cherrySeeder.Seed();
        }
    }
}