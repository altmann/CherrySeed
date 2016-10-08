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

        public SeederConfiguration()
        {
            TypeTransformations = new Dictionary<Type, ITypeTransformation>
            {
                { typeof(string), new StringTransformation("$EMPTY$") },
                { typeof(int), new IntegerTransformation() },
                { typeof(DateTime), new DateTimeTransformation() },
                { typeof(bool), new BooleanTransformation() },
                { typeof(Guid), new GuidTransformation() },
                { typeof(Enum), new EnumTransformation() },
                { typeof(double), new DoubleTransformation() },
                { typeof(decimal), new DecimalTransformation() }
            };

            // set defaults
            DefaultPrimaryKeyNames = new List<string> { "Id", "{ClassName}Id" };
            DefaultIdGeneration = new IdGenerationSetting();
            IsClearBeforeSeedingEnabled = true;
        }
    }
}