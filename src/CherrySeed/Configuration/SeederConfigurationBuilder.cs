using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDataProvider;
using CherrySeed.EntitySettings;
using CherrySeed.PrimaryKeyIdGeneration;
using CherrySeed.Repositories;
using CherrySeed.TypeTransformations;

namespace CherrySeed.Configuration
{
    public class SeederConfigurationBuilder : ISeederConfigurationBuilder
    {
        private readonly List<EntitySettingBuilder> _entitySettingBuilders;
        private readonly SeederConfiguration _seederConfiguration;
        private int _order = 1;

        public SeederConfigurationBuilder()
        {
            _entitySettingBuilders = new List<EntitySettingBuilder>();
            _seederConfiguration = new SeederConfiguration();
        }

        public SeederConfiguration Build()
        {
            _seederConfiguration.EntitySettings = _entitySettingBuilders
                .Select(b => b.Build(
                    _seederConfiguration.DefaultRepository, 
                    _seederConfiguration.DefaultIdGeneration))
                .ToList();

            return _seederConfiguration;
        }

        public IEntitySettingBuilder<T> ForEntity<T>()
        {
            var entityType = typeof (T);
            var newObjectDescriptionBuilder = new EntitySettingBuilder<T>(entityType, _order++);
            _entitySettingBuilders.Add(newObjectDescriptionBuilder);
            return newObjectDescriptionBuilder;
        }

        public void WithDataProvider(IDataProvider dataProvider)
        {
            _seederConfiguration.DataProvider = dataProvider;
        }

        public void AddTypeTransformation(Type type, ITypeTransformation transformation)
        {
            if (_seederConfiguration.TypeTransformations.ContainsKey(type))
            {
                _seederConfiguration.TypeTransformations[type] = transformation;
            }
            else
            {
                _seederConfiguration.TypeTransformations.Add(type, transformation);
            }
        }

        public void WithDefaultPrimaryKeyNames(params string[] primaryKeyNames)
        {
            _seederConfiguration.DefaultPrimaryKeyNames.AddRange(primaryKeyNames);
        }

        public void WithRepository(IRepository repository)
        {
            _seederConfiguration.DefaultRepository = repository;
        }

        public void DisableClearBeforeSeeding()
        {
            _seederConfiguration.IsClearBeforeSeedingEnabled = false;
        }

        public void BeforeSave(Action<Dictionary<string, string>, object> beforeSaveAction)
        {
            _seederConfiguration.BeforeSaveAction = beforeSaveAction;
        }

        public void AfterSave(Action<Dictionary<string, string>, object> afterSaveAction)
        {
            _seederConfiguration.AfterSaveAction = afterSaveAction;
        }

        public void WithPrimaryKeyIdGenerationInApplicationAsInteger(int startId = 1, int steps = 1)
        {
            _seederConfiguration.DefaultIdGeneration = new IdGenerationSetting(new IntegerPrimaryKeyIdGenerator(startId, steps));
        }

        public void WithPrimaryKeyIdGenerationInApplicationAsGuid()
        {
            _seederConfiguration.DefaultIdGeneration = new IdGenerationSetting(new GuidPrimaryKeyIdGenerator());
        }

        public void WithPrimaryKeyIdGenerationInApplicationAsString(string prefix = "", int startId = 1, int steps = 1)
        {
            _seederConfiguration.DefaultIdGeneration = new IdGenerationSetting(new StringPrimaryKeyIdGenerator(prefix, startId, steps));
        }

        public void WithCustomPrimaryKeyIdGenerationInApplication(IPrimaryKeyIdGenerator generator)
        {
            _seederConfiguration.DefaultIdGeneration = new IdGenerationSetting(generator);
        }

        public void WithEmptyStringMarker(string marker)
        {
            _seederConfiguration.TypeTransformations[typeof(string)] = new StringTransformation(marker);
        }
    }
}