using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.Configuration.Exceptions;
using CherrySeed.EntityDataProvider;
using CherrySeed.EntitySettings;
using CherrySeed.PrimaryKeyIdGeneration;
using CherrySeed.Repositories;
using CherrySeed.TypeTransformations;
using CherrySeed.Utils;

namespace CherrySeed.Configuration
{
    public class SeederConfigurationBuilder : ISeederConfigurationBuilder
    {
        private readonly Dictionary<Type, EntitySettingBuilder> _entitySettingBuilders;
        private int _order = 1;

        // Settings
        public string EmptyStringMarker { get; set; }
        public List<EntitySetting> EntitySettings { get; set; }
        public List<string> DefaultPrimaryKeyNames { get; set; }
        public IRepository DefaultRepository { get; set; }
        public IdGenerationSetting DefaultIdGeneration { get; set; }
        public bool IsClearBeforeSeedingEnabled { get; set; }
        public Action<Dictionary<string, string>, object> BeforeSaveAction { get; set; }
        public Action<Dictionary<string, string>, object> AfterSaveAction { get; set; }
        public IDataProvider DataProvider { get; set; }
        public Dictionary<Type, ITypeTransformation> TypeTransformations { get; set; }

        public SeederConfigurationBuilder()
        {
            _entitySettingBuilders = new Dictionary<Type, EntitySettingBuilder>();

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
            DefaultPrimaryKeyNames = new List<string> { "Id", "ID", "{ClassName}Id" };
            DefaultIdGeneration = new IdGenerationSetting();
            IsClearBeforeSeedingEnabled = true;
        }

        public SeederConfiguration Build()
        {
            var entitySettings = _entitySettingBuilders.Values
                .Select(b => b.Build(
                    DefaultRepository, 
                    DefaultIdGeneration,
                    DefaultPrimaryKeyNames))
                .ToList();

            if (!string.IsNullOrEmpty(EmptyStringMarker))
            { 
                if (!(TypeTransformations[typeof(string)] is StringTransformation))
                {
                    throw new ConfigurationException("EmptyString marker can not be set, because the string transformation logic is overriden from you.", null);
                }

                TypeTransformations[typeof(string)] = new StringTransformation(EmptyStringMarker);
            }
            
            var configuration = new SeederConfiguration
            {
                AfterSaveAction = AfterSaveAction,
                BeforeSaveAction = BeforeSaveAction,
                DataProvider = DataProvider,
                DefaultIdGeneration = DefaultIdGeneration,
                DefaultPrimaryKeyNames = DefaultPrimaryKeyNames,
                DefaultRepository = DefaultRepository,
                EntitySettings = entitySettings,
                IsClearBeforeSeedingEnabled = IsClearBeforeSeedingEnabled,
                TypeTransformations = TypeTransformations
            };

            var configurationValidator = new SeederConfigurationValidator();
            configurationValidator.IsValid(configuration);

            return configuration;
        }

        public IEntitySettingBuilder<T> ForEntity<T>()
        {
            var entityType = typeof(T);

            if (_entitySettingBuilders.ContainsKey(entityType))
            {
                return _entitySettingBuilders[entityType] as IEntitySettingBuilder<T>;
            }

            var entitySettingBuilder = new EntitySettingBuilder<T>(entityType, _order++);
            _entitySettingBuilders.Add(entityType, entitySettingBuilder);

            return entitySettingBuilder;
        }

        public void WithDataProvider(IDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        public void AddTypeTransformation(Type type, ITypeTransformation transformation)
        {
            TypeTransformations.AddOrReplace(type, transformation);
        }

        public void WithDefaultPrimaryKeyNames(params string[] primaryKeyNames)
        {
            DefaultPrimaryKeyNames.AddRange(primaryKeyNames);
        }

        public void WithRepository(IRepository repository)
        {
            DefaultRepository = repository;
        }

        public void DisableClearBeforeSeeding()
        {
            IsClearBeforeSeedingEnabled = false;
        }

        public void BeforeSave(Action<Dictionary<string, string>, object> beforeSaveAction)
        {
            BeforeSaveAction = beforeSaveAction;
        }

        public void AfterSave(Action<Dictionary<string, string>, object> afterSaveAction)
        {
            AfterSaveAction = afterSaveAction;
        }

        public void DisablePrimaryKeyIdGeneration()
        {
            DefaultIdGeneration.IsGeneratorEnabled = false;
            DefaultIdGeneration.Generator = null;
        }

        public void WithPrimaryKeyIdGenerationInApplicationAsInteger(int startId = 1, int steps = 1)
        {
            DefaultIdGeneration.Generator = new IntegerPrimaryKeyIdGenerator(startId, steps);
        }

        public void WithPrimaryKeyIdGenerationInApplicationAsGuid()
        {
            DefaultIdGeneration.Generator = new GuidPrimaryKeyIdGenerator();
        }

        public void WithPrimaryKeyIdGenerationInApplicationAsString(string prefix = "", int startId = 1, int steps = 1)
        {
            DefaultIdGeneration.Generator = new StringPrimaryKeyIdGenerator(prefix, startId, steps);
        }

        public void WithCustomPrimaryKeyIdGenerationInApplication(IPrimaryKeyIdGenerator generator)
        {
            DefaultIdGeneration.Generator = generator;
        }

        public void WithEmptyStringMarker(string marker)
        {
            EmptyStringMarker = marker;
        }
    }
}