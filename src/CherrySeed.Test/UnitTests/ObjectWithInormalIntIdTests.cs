using System;
using System.Collections.Generic;
using CherrySeed.EntityDataProvider;
using CherrySeed.EntitySettings;
using CherrySeed.Repositories;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.UnitTests
{
    [TestClass]
    public class ObjectWithInormalIntIdTests
    {
        [TestMethod]
        public void ObjectWithInormalIntId()
        {
            var entityData = new List<EntityData>
            {
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.Project",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "MyProjectId", "P1" },
                            { "CustomerId", "C1" }
                        },
                        new Dictionary<string, string>
                        {
                            { "MyProjectId", "P2" },
                            { "CustomerId", "C2" }
                        }
                    }
                },
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.Customer",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "MyCustomerId", "C1" }
                        },
                        new Dictionary<string, string>
                        {
                            { "MyCustomerId", "C2" }
                        }
                    }
                },
            };

            var assertRepository = new AssertRepository((obj, count) =>
            {
                AssertHelper.AssertIf(typeof(Project), 0, count, obj, () =>
                {
                    AssertProject.AssertProperties(obj, 1);
                });

                AssertHelper.AssertIf(typeof(Project), 1, count, obj, () =>
                {
                    AssertProject.AssertProperties(obj, 2);
                });

            }, type =>
            {
                
            });
            
            InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.ForEntity<Customer>()
                    .WithPrimaryKey(e => e.MyCustomerId)
                    .WithIntegerIdGenerationViaCode();

                cfg.ForEntity<Project>()
                    .WithPrimaryKey(e => e.MyProjectId)
                    .WithReference(e => e.CustomerId, typeof (Customer))
                    .WithIntegerIdGenerationViaCode();
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