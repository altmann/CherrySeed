using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.Configuration;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Base.Asserts;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Convert;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    [TestClass]
    public class BasicTests
    {
        private readonly CherrySeedDriver _cherrySeedDriver;

        public BasicTests()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void EntityWithIntId()
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
                            {"Id", "1"},
                            {"MyString", "MyString 1"},
                            {"MyBool", "true"},
                            {"MyDateTime", "2016/05/03"},
                            {"MyDouble", "123,456"},
                            {"MyDecimal", "123,12"},
                            {"MyEnum1", "EnumValue1"},
                            {"MyEnum2", "EnumValue2"},
                        },
                        new Dictionary<string, string>
                        {
                            {"Id", "2"},
                            {"MyString", "MyString 2"},
                            {"MyBool", "false"},
                            {"MyDateTime", "2016/04/05"},
                            {"MyDouble", "123,998"},
                            {"MyDecimal", "1,43"},
                            {"MyEnum1", "0"},
                            {"MyEnum2", "1"},
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
                            {"Id", "1"},
                            {"MyString", "MyString 1"},
                        },
                        new Dictionary<string, string>
                        {
                            {"Id", "2"},
                            {"MyString", "MyString 2"},
                        }
                    }
                },
            };

            var assertRepository = new AssertRepository((obj, count, entities) =>
            {
                AssertHelper.AssertIf(typeof (Main), 0, count, obj, () =>
                {
                    AssertMain.AssertProperties(obj, new DateTime(2016, 5, 3), "MyString 1", true, (decimal) 123.12,
                        123.456, MyEnum.EnumValue1, MyEnum.EnumValue2);
                });

                AssertHelper.AssertIf(typeof (Main), 1, count, obj, () =>
                {
                    AssertMain.AssertProperties(obj, new DateTime(2016, 4, 5), "MyString 2", false, (decimal) 1.43,
                        123.998, MyEnum.EnumValue1, MyEnum.EnumValue2);
                });

                AssertHelper.AssertIf(typeof (Sub), 0, count, obj, () =>
                {
                    AssertSub.AssertProperties(obj, "MyString 1");
                });

                AssertHelper.AssertIf(typeof (Sub), 1, count, obj, () =>
                {
                    AssertSub.AssertProperties(obj, "MyString 2");
                });
            }, (type, repo) =>
            {

            }, null);

            _cherrySeedDriver.InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.ForEntity<Sub>()
                    .WithPrimaryKey(e => e.Id)
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<Main>()
                    .AfterSave(obj => { });
            });
        }

        [TestMethod]
        public void EntityWithIntId_EntitySettingsBeforeGlobalSettings()
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
                            {"Id", "1"},
                            {"MyString", "MyString 1"},
                            {"MyBool", "true"},
                            {"MyDateTime", "2016/05/03"},
                            {"MyDouble", "123,456"},
                            {"MyDecimal", "123,12"},
                            {"MyEnum1", "EnumValue1"},
                            {"MyEnum2", "EnumValue2"},
                        },
                        new Dictionary<string, string>
                        {
                            {"Id", "2"},
                            {"MyString", "MyString 2"},
                            {"MyBool", "false"},
                            {"MyDateTime", "2016/04/05"},
                            {"MyDouble", "123,998"},
                            {"MyDecimal", "1,43"},
                            {"MyEnum1", "0"},
                            {"MyEnum2", "1"},
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
                            {"Id", "1"},
                            {"MyString", "MyString 1"},
                        },
                        new Dictionary<string, string>
                        {
                            {"Id", "2"},
                            {"MyString", "MyString 2"},
                        }
                    }
                },
            };

            var assertRepository = new AssertRepository((obj, count, entities) =>
            {
                AssertHelper.AssertIf(typeof(Main), 0, count, obj, () =>
                {
                    AssertMain.AssertProperties(obj, new DateTime(2016, 5, 3), "MyString 1", true, (decimal)123.12,
                        123.456, MyEnum.EnumValue1, MyEnum.EnumValue2);
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

            var config = new CherrySeedConfiguration(cfg =>
            {
                cfg.ForEntity<Sub>()
                    .WithPrimaryKey(e => e.Id)
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<Main>()
                    .AfterSave(obj => { });

                cfg.WithDataProvider(new DictionaryDataProvider(entityData));
                cfg.WithRepository(assertRepository);
            });

            var cherrySeeder = config.CreateSeeder();
            cherrySeeder.Seed();
        }

        [TestMethod]
        public void EntityWithGuidId()
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
                            {"PersonId", "P1"},
                            {"AddressId", "A1"}
                        },
                        new Dictionary<string, string>
                        {
                            {"PersonId", "P2"},
                            {"AddressId", "A1"}
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
                            {"AddressId", "A1"}
                        },
                        new Dictionary<string, string>
                        {
                            {"AddressId", "A2"}
                        }
                    }
                },
            };

            var assertRepository = new AssertRepository((obj, count, entities) =>
            {
                AssertHelper.AssertIf(typeof (Person), 0, count, obj, () =>
                {
                    AssertPerson.AssertProperties(obj, Converter.ToGuid(1));
                });

                AssertHelper.AssertIf(typeof (Person), 1, count, obj, () =>
                {
                    AssertPerson.AssertProperties(obj, Converter.ToGuid(1));
                });

            }, (type, repo) =>
            {

            }, null);

            _cherrySeedDriver.InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.ForEntity<Address>()
                    .WithCustomPrimaryKeyIdGenerationInApplication(new SequentialGuidPrimaryKeyIdGenerator());

                cfg.ForEntity<Person>()
                    .WithReference(e => e.AddressId, typeof (Address))
                    .WithCustomPrimaryKeyIdGenerationInApplication(new SequentialGuidPrimaryKeyIdGenerator());
            });
        }

        [TestMethod]
        public void EntityWithStringId()
        {
            var entityData = new List<EntityData>
            {
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.EntityWithStringId",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            {"Id", "1"},
                            {"AnotherEntityWithStringIdId", "A1"}
                        },
                        new Dictionary<string, string>
                        {
                            {"Id", "2"},
                            {"AnotherEntityWithStringIdId", "A2"}
                        }
                    }
                },
                new EntityData
                {
                    EntityName = "CherrySeed.Test.Models.AnotherEntityWithStringId",
                    Objects = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            {"Id", "A1"}
                        },
                        new Dictionary<string, string>
                        {
                            {"Id", "A2"}
                        }
                    }
                },
            };

            var assertRepository = new AssertRepository((obj, count, entities) =>
            {
                AssertHelper.AssertIf(typeof(EntityWithStringId), 0, count, obj, () =>
                {
                    AssertEntityWithStringId.AssertProperties(obj, "E1-1000", "E2-100");
                });

                AssertHelper.AssertIf(typeof(EntityWithStringId), 1, count, obj, () =>
                {
                    AssertEntityWithStringId.AssertProperties(obj, "E1-1100", "E2-200");
                });

                AssertHelper.AssertIf(typeof(AnotherEntityWithStringId), 0, count, obj, () =>
                {
                    AssertAnotherEntityWithStringId.AssertProperties(obj, "E2-100");
                });

                AssertHelper.AssertIf(typeof(AnotherEntityWithStringId), 1, count, obj, () =>
                {
                    AssertAnotherEntityWithStringId.AssertProperties(obj, "E2-200");
                });

            }, (type, repo) =>
            {

            }, null);

            _cherrySeedDriver.InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.ForEntity<AnotherEntityWithStringId>()
                    .WithPrimaryKeyIdGenerationInApplicationAsString("E2-", 100, 100);

                cfg.ForEntity<EntityWithStringId>()
                    .WithReference(e => e.AnotherEntityWithStringIdId, typeof(AnotherEntityWithStringId))
                    .WithPrimaryKeyIdGenerationInApplicationAsString("E1-", 1000, 100);
            });
        }

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

            var assertRepository = new AssertRepository((obj, count, entities) =>
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
            }, (type, repo) =>
            {

            }, null);

            _cherrySeedDriver.InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.ForEntity<Sub>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<NullableMain>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();
            });
        }

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

            var assertRepository = new AssertRepository((obj, count, repo) =>
            {
                AssertHelper.AssertIf(typeof(MainWithModelReference), 0, count, obj, () =>
                {
                    AssertMainWithModelReference.AssertProperties(obj, repo.Entities[typeof(Sub)].First(e => ((Sub)e).Id == 2));
                });

                AssertHelper.AssertIf(typeof(MainWithModelReference), 1, count, obj, () =>
                {
                    AssertMainWithModelReference.AssertProperties(obj, repo.Entities[typeof(Sub)].First(e => ((Sub)e).Id == 1));
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

            }, (entities, type, id) =>
            {
                if (!entities.ContainsKey(type))
                    throw new InvalidOperationException("entity type not found in dict");

                if (type != typeof(Sub))
                    throw new InvalidOperationException("this entity is not supported for model reference");

                return entities[type].First(e => ((Sub)e).Id == (int)id);
            });

            _cherrySeedDriver.InitAndExecute(entityData, assertRepository, cfg =>
            {
                cfg.ForEntity<Sub>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<MainWithModelReference>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger()
                    .WithReference(e => e.Sub, typeof(Sub));
            });
        }
    }
}