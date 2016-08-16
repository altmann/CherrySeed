using System;
using System.Collections.Generic;
using CherrySeed.Configuration;
using CherrySeed.EntityDataProvider;
using CherrySeed.Repositories;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using CherrySeed.TypeTransformations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.UnitTests
{
    public class CustomStringTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return "Michael";
        }

        public object TransformNullable(Type type, string str)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class ObjectWithCustomStringTransformationTests
    {
        [TestMethod]
        public void ObjectWithCustomStringTransformation()
        {
            var entityData = new List<EntityData>
            {
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.Sub",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "Id", "1" },
                            { "MyString", "Hello" },
                        },
                        new Dictionary<string, string>
                        {
                            { "Id", "2" },
                            { "MyString", "World" },
                        }
                    }
                },
            };

            var assertRepository = new AssertRepository((obj, count, entities) =>
            {
                AssertHelper.AssertIf(typeof(Sub), 0, count, obj, () =>
                {
                    AssertSub.AssertProperties(obj, "Michael");
                });

                AssertHelper.AssertIf(typeof(Sub), 1, count, obj, () =>
                {
                    AssertSub.AssertProperties(obj, "Michael");
                });
            }, type =>
            {
                
            });
            
            InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.AddTypeTransformation(typeof(string), new CustomStringTransformation());

                cfg.ForEntity<Sub>()
                    .WithIntegerIdGenerationViaCode();
            });
        }

        private void InitAndExecute(List<EntityData> data, IRepository repository, 
            Action<ISeederConfigurationBuilder> entitySettings)
        {
            var config = new CherrySeedConfiguration(cfg =>
            {
                cfg.WithDataProvider(new DictionaryDataProvider(data));
                cfg.WithRepository(repository);

                entitySettings(cfg);
            });

            var cherrySeeder = config.CreateSeeder();
            cherrySeeder.Seed();
        }
    }
}