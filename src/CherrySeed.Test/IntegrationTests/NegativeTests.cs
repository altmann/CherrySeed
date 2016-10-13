using System;
using System.Collections.Generic;
using CherrySeed.Configuration;
using CherrySeed.Configuration.Exceptions;
using CherrySeed.EntityDataProvider;
using CherrySeed.ObjectTransformation;
using CherrySeed.Test.Asserts;
using CherrySeed.Test.Base.Asserts;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.Test.Infrastructure;
using CherrySeed.Test.Mocks;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.IntegrationTests
{
    [TestClass]
    public class NegativeTests
    {
        private readonly CherrySeedDriver _cherrySeedDriver;

        public NegativeTests()
        {
            _cherrySeedDriver = new CherrySeedDriver();
        }

        [TestMethod]
        public void DataProviderNotSet_MissingConfigurationException()
        {
            AssertHelper.TryCatch(tryAction: () =>
            {
                var config = new CherrySeedConfiguration(cfg =>
                {
                    cfg.WithRepository(new EmptyRepository());
                });

                config.CreateSeeder();
            }, catchAction: ex =>
            {
                AssertHelper.AssertExceptionWithMessage(ex, typeof(MissingConfigurationException), "DataProvider");
            });
        }

        [TestMethod]
        public void RepositoryNotSet_MissingConfigurationException()
        {
            AssertHelper.TryCatch(tryAction: () =>
            {
                var config = new CherrySeedConfiguration(cfg =>
                {
                    cfg.WithDataProvider(new DictionaryDataProvider(new List<EntityData>()));
                });

                config.CreateSeeder();
            }, catchAction: ex =>
            {
                AssertHelper.AssertExceptionWithMessage(ex, typeof(MissingConfigurationException), "Repository");
            });
        }

        [TestMethod]
        public void IncorrectPropertyName_PropertyMappingException_MissingProperty()
        {
            AssertHelper.TryCatch(tryAction: () =>
            {
                var entityData = new List<EntityData>
                {
                    new EntityData
                    {
                        EntityName = "CherrySeed.Test.Models.Simple",
                        Objects = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                {"Integer", "1"}
                            },
                            new Dictionary<string, string>
                            {
                                {"IncorrectFieldName", "2"}
                            }
                        }
                    }
                };
                
                _cherrySeedDriver.InitAndExecute(entityData, new EmptyRepository(), cfg =>
                {
                    cfg.ForEntity<Simple>();
                });
            }, catchAction: ex =>
            {
                AssertHelper.AssertExceptionWithMessage(ex, typeof(PropertyTransformationException), "Transformation of Property 'IncorrectFieldName' of type 'CherrySeed.Test.Models.Simple' to value '2' failed");
                AssertHelper.AssertExceptionWithMessage(ex.InnerException, typeof(NullReferenceException), "Property is missing");
            });
        }

        [TestMethod]
        public void IncorrectPropertyType_PropertyMappingException_MissingProperty()
        {
            AssertHelper.TryCatch(tryAction: () =>
            {
                var entityData = new List<EntityData>
                {
                    new EntityData
                    {
                        EntityName = "CherrySeed.Test.Models.Simple",
                        Objects = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                {"Integer", "1"}
                            },
                            new Dictionary<string, string>
                            {
                                {"Integer", "NotANumber"}
                            }
                        }
                    }
                };

                _cherrySeedDriver.InitAndExecute(entityData, new EmptyRepository(), cfg =>
                {
                    cfg.ForEntity<Simple>();
                });
            }, catchAction: ex =>
            {
                AssertHelper.AssertExceptionWithMessage(ex, typeof(PropertyTransformationException), "Transformation of Property 'Integer' of type 'CherrySeed.Test.Models.Simple' to value 'NotANumber' failed");
                AssertHelper.AssertException(ex.InnerException, typeof(FormatException));
            });
        }
    }
}