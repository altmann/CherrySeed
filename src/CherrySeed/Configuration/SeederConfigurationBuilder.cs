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
        private string _emptyStringMarker;
        private readonly List<string> _defaultPrimaryKeyNames;
        private IRepository _defaultRepository;
        private readonly IdGenerationSetting _defaultIdGeneration;
        private bool _isClearBeforeSeedingEnabled;
        private Action<Dictionary<string, string>, object> _beforeSaveAction;
        private Action<Dictionary<string, string>, object> _afterSaveAction;
        private IDataProvider _dataProvider;
        private readonly Dictionary<Type, ITypeTransformation> _typeTransformations;

        public SeederConfigurationBuilder()
        {
            _entitySettingBuilders = new Dictionary<Type, EntitySettingBuilder>();

            _typeTransformations = new Dictionary<Type, ITypeTransformation>
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
            _defaultPrimaryKeyNames = new List<string> { "Id", "ID", "{ClassName}Id" };
            _defaultIdGeneration = new IdGenerationSetting();
            _isClearBeforeSeedingEnabled = true;
        }

        public SeederConfiguration Build()
        {
            var entitySettings = _entitySettingBuilders.Values
                .Select(b => b.Build(
                    _defaultRepository, 
                    _defaultIdGeneration,
                    _defaultPrimaryKeyNames))
                .ToList();

            if (!string.IsNullOrEmpty(_emptyStringMarker))
            { 
                if (!(_typeTransformations[typeof(string)] is StringTransformation))
                {
                    throw new ConfigurationException("EmptyString marker can not be set, because the string transformation logic is overriden from you.", null);
                }

                _typeTransformations[typeof(string)] = new StringTransformation(_emptyStringMarker);
            }
            
            var configuration = new SeederConfiguration
            {
                AfterSaveAction = _afterSaveAction,
                BeforeSaveAction = _beforeSaveAction,
                DataProvider = _dataProvider,
                DefaultIdGeneration = _defaultIdGeneration,
                DefaultPrimaryKeyNames = _defaultPrimaryKeyNames,
                DefaultRepository = _defaultRepository,
                EntitySettings = entitySettings,
                IsClearBeforeSeedingEnabled = _isClearBeforeSeedingEnabled,
                TypeTransformations = _typeTransformations
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
            _dataProvider = dataProvider;
        }

        public void AddTypeTransformation(Type type, ITypeTransformation transformation)
        {
            _typeTransformations.AddOrReplace(type, transformation);
        }

        public void WithDefaultPrimaryKeyNames(params string[] primaryKeyNames)
        {
            _defaultPrimaryKeyNames.AddRange(primaryKeyNames);
        }

        public void WithRepository(IRepository repository)
        {
            _defaultRepository = repository;
        }

        public void DisableClearBeforeSeeding()
        {
            _isClearBeforeSeedingEnabled = false;
        }

        public void BeforeSave(Action<Dictionary<string, string>, object> beforeSaveAction)
        {
            _beforeSaveAction = beforeSaveAction;
        }

        public void AfterSave(Action<Dictionary<string, string>, object> afterSaveAction)
        {
            _afterSaveAction = afterSaveAction;
        }

        public void DisablePrimaryKeyIdGeneration()
        {
            _defaultIdGeneration.IsGeneratorEnabled = false;
            _defaultIdGeneration.Generator = null;
        }

        public void WithPrimaryKeyIdGenerationInApplicationAsInteger(int startId = 1, int steps = 1)
        {
            _defaultIdGeneration.Generator = new IntegerPrimaryKeyIdGenerator(startId, steps);
        }

        public void WithPrimaryKeyIdGenerationInApplicationAsGuid()
        {
            _defaultIdGeneration.Generator = new GuidPrimaryKeyIdGenerator();
        }

        public void WithPrimaryKeyIdGenerationInApplicationAsString(string prefix = "", int startId = 1, int steps = 1)
        {
            _defaultIdGeneration.Generator = new StringPrimaryKeyIdGenerator(prefix, startId, steps);
        }

        public void WithCustomPrimaryKeyIdGenerationInApplication(IPrimaryKeyIdGenerator generator)
        {
            _defaultIdGeneration.Generator = generator;
        }

        public void WithEmptyStringMarker(string marker)
        {
            _emptyStringMarker = marker;
        }
    }
}