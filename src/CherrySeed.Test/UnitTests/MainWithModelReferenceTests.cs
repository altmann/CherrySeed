using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.Configuration;
using CherrySeed.EntityDataProvider;
using CherrySeed.Repositories;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.UnitTests
{
    [TestClass]
    public class MainWithModelReferenceTests
    {
        [TestMethod]
        public void MainWithModelReference()
        {
            var entityData = new List<EntityData>
            {
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.MainWithModelReference",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "Id", "1" },
                            { "Sub", "2" },
                        },
                        new Dictionary<string, string>
                        {
                            { "Id", "2" },
                            { "Sub", "1" },
                        }
                    }
                },
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.Sub",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "Id", "1" },
                            { "MyString", "MyString 1" },
                        },
                        new Dictionary<string, string>
                        {
                            { "Id", "2" },
                            { "MyString", "MyString 2" },
                        }
                    }
                },
            };

            var assertRepository = new AssertRepository((obj, count, entities) =>
            {
                AssertHelper.AssertIf(typeof(MainWithModelReference), 0, count, obj, () =>
                {
                    AssertMainWithModelReference.AssertProperties(obj, entities[typeof(Sub)].First(e => ((Sub)e).Id == 2));
                });

                AssertHelper.AssertIf(typeof(MainWithModelReference), 1, count, obj, () =>
                {
                    AssertMainWithModelReference.AssertProperties(obj, entities[typeof(Sub)].First(e => ((Sub)e).Id == 1));
                });

                AssertHelper.AssertIf(typeof(Sub), 0, count, obj, () =>
                {
                    AssertSub.AssertProperties(obj, "MyString 1");
                });

                AssertHelper.AssertIf(typeof(Sub), 1, count, obj, () =>
                {
                    AssertSub.AssertProperties(obj, "MyString 2");
                });
            }, type =>
            {
                
            });
            
            InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.ForEntity<Sub>()
                    .WithIntegerIdGenerationViaCode();

                cfg.ForEntity<MainWithModelReference>()
                    .WithIntegerIdGenerationViaCode()
                    .WithReference(e => e.Sub, typeof(Sub));
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