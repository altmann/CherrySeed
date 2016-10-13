using System;
using System.Collections.Generic;
using CherrySeed.Configuration;
using CherrySeed.EntityDataProvider;
using CherrySeed.Repositories;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Base.Asserts;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    [TestClass]
    public class ObjectWithDefaultValuesTests
    {
        [TestMethod]
        public void ObjectWithDefaultValues()
        {
            var entityData = new List<EntityData>
            {
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.Main",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "Id", "1" },
                            { "MyString", "MyString 1" },
                            { "MyDouble", "123,456" },
                            { "MyEnum1", "EnumValue1" },
                            { "MyEnum2", "EnumValue2" },
                        },
                        new Dictionary<string, string>
                        {
                            { "Id", "2" },
                            { "MyString", "MyString 2" },
                            { "MyBool", "false" },
                            { "MyDateTime", "2016/04/05" },
                            { "MyDouble", "123,998" },
                            { "MyDecimal", "1,43" },
                            { "MyEnum1", "0" },
                            { "MyEnum2", "1" },
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
                AssertHelper.AssertIf(typeof(Main), 0, count, obj, () =>
                {
                    AssertMain.AssertProperties(obj, new DateTime(2010, 1, 1), "MyString 1", true, (decimal)12.12, 123.456, MyEnum.EnumValue1, MyEnum.EnumValue2);
                });

                AssertHelper.AssertIf(typeof(Main), 1, count, obj, () =>
                {
                    AssertMain.AssertProperties(obj, new DateTime(2016, 4, 5), "MyString 2", false, (decimal)1.43,
                            123.998, MyEnum.EnumValue1, MyEnum.EnumValue2);
                });

                AssertHelper.AssertIf(typeof(Sub), 0, count, obj, () =>
                {
                    AssertSub.AssertProperties(obj, "MyString 1");
                });

                AssertHelper.AssertIf(typeof(Sub), 1, count, obj, () =>
                {
                    AssertSub.AssertProperties(obj, "MyString 2");
                });
            }, (type, repo) =>
            {
                
            }, null);
            
            InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.ForEntity<Sub>()
                    .WithPrimaryKey(e => e.Id)
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<Main>()
                    .WithFieldWithDefaultValue(e => e.MyBool, () => true)
                    .WithFieldWithDefaultValue(e => e.MyDateTime, () => new DateTime(2010, 1, 1))
                    .WithFieldWithDefaultValue(e => e.MyDecimal, new DefaultValueProviderMock<decimal>((decimal)12.12));
            });
        }

        private void InitAndExecute(List<EntityData> data, IRepository repository, 
            Action<ISeederConfigurationBuilder> entitySettings)
        {
            var config = new CherrySeedConfiguration(cfg =>
            {
                cfg.WithDataProvider(new DictionaryDataProvider(data));
                cfg.WithRepository(repository);

                cfg.BeforeSave((objDictionary, obj) =>
                {
                    
                });

                entitySettings(cfg);
            });

            var cherrySeeder = config.CreateSeeder();
            cherrySeeder.Seed();
        }
    }
}