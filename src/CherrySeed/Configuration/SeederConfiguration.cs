using System;
using System.Collections.Generic;
using CherrySeed.EntityDataProvider;
using CherrySeed.EntitySettings;
using CherrySeed.Repositories;
using CherrySeed.TypeTransformations;

namespace CherrySeed.Configuration
{
    public class SeederConfiguration
    {
        public List<EntitySetting> EntitySettings { get; set; }
        public List<string> DefaultPrimaryKeyNames { get; set; }
        public IRepository DefaultRepository { get; set; }
        public IdGenerationSetting DefaultIdGeneration { get; set; }
        public bool IsClearBeforeSeedingEnabled { get; set; }
        public Action<Dictionary<string, string>, object> BeforeSaveAction { get; set; }
        public Action<Dictionary<string, string>, object> AfterSaveAction { get; set; }
        public IDataProvider DataProvider { get; set; }
        public Dictionary<Type, ITypeTransformation> TypeTransformations { get; set; }
    }
}