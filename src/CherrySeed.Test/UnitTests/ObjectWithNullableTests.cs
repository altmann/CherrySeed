using System;
using System.Collections.Generic;
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
    public class ObjectWithNullableTests
    {
        [TestMethod]
        public void ObjectWithNullable()
        {
            var entityData = new List<EntityData>
            {
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.NullableMain",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "Id", "1" },
                            { "SubId", "" },
                            { "MyInt", "" },
                            { "MyString", "" },
                            { "MyString2", "$EMPTY$" },
                            { "MyBool", "" },
                            { "MyDateTime", "" },
                            { "MyDouble", "" },
                            { "MyDecimal", "" },
                            { "MyEnum1", "" },
                            { "MyEnum2", "" },
                        },
                        new Dictionary<string, string>
                        {
                            { "Id", "2" },
                            { "SubId", "2" },
                            { "MyInt", "2" },
                            { "MyString", "MyString 2" },
                            { "MyString2", "MyString 2" },
                            { "MyBool", "false" },
                            { "MyDateTime", "2016/04/05" },
                            { "MyDouble", "123,998" },
                            { "MyDecimal", "1,43" },
                            { "MyEnum1", "0" },
                            { "MyEnum2", "1" },
                        },
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

            var assertRepository = new AssertRepository((obj, count) =>
            {
                AssertHelper.AssertIf(typeof(NullableMain), 0, count, obj, () =>
                {
                    AssertNullableMain.AssertProperties(obj, null, null, null, "", null, null, null, null, null, null);
                });

                AssertHelper.AssertIf(typeof(NullableMain), 1, count, obj, () =>
                {
                    AssertNullableMain.AssertProperties(obj, 2, new DateTime(2016, 4, 5), "MyString 2", "MyString 2", false, (decimal)1.43,
                            123.998, MyEnum.EnumValue1, MyEnum.EnumValue2, 2);
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

                cfg.ForEntity<NullableMain>()
                    .WithIntegerIdGenerationViaCode();
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