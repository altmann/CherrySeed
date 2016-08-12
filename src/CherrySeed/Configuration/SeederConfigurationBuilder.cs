using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDataProvider;
using CherrySeed.EntitySettings;
using CherrySeed.IdGeneration;
using CherrySeed.Repositories;
using CherrySeed.TypeTransformations;

namespace CherrySeed.Configuration
{
    public interface ISeederConfigurationBuilder
    {
        EntitySettingBuilder<T> ForEntity<T>();
        void WithDataProvider(IEntityDataProvider dataProvider);
        void AddTypeTransformation(Type type, ITypeTransformation transformation);
        void WithDefaultPrimaryKeyNames(params string[] primaryKeyNames);
        void WithDefaultRepository(IRepository repository);
        void DisableClearBeforeSeeding();
        void AfterTransformation(Action<Dictionary<string, string>, object> afterTransformationAction);
        void WithIntegerIdGenerationViaCode(int startId = 1, int steps = 1);
        void WithGuidIdGenerationViaCode();
        void WithCustomIdGenerationViaCode(IIdGenerator generator);
    }

    public class SeederConfigurationBuilder : ISeederConfigurationBuilder
    {
        private readonly List<EntitySettingBuilder> _entitySettingBuilders;
        private int _order = 1;

        public List<EntitySetting> EntitySettings
        {
            get
            {
                var builders = _entitySettingBuilders;
                return builders.Select(b => b.Build()).ToList();
            }
        }

        public PrimaryKeySetting DefaultPrimaryKey { get; set; }
        public IRepository DefaultRepository { get; set; }
        public IdGenerationSetting DefaultIdGeneration { get; set; }
        public bool IsClearBeforeSeedingEnabled { get; set; }
        public Action<Dictionary<string, string>, object> AfterTransformationAction { get; set; }
        public IEntityDataProvider DataProvider { get; set; }
        public Dictionary<Type, ITypeTransformation> TypeTransformations { get; }

        public SeederConfigurationBuilder()
        {
            // init
            _entitySettingBuilders = new List<EntitySettingBuilder>();
            TypeTransformations = new Dictionary<Type, ITypeTransformation>();
            
            // set defaults
            DefaultPrimaryKey = new PrimaryKeySetting(new List<string> { "Id", "{ClassName}Id" });
            DefaultIdGeneration = new IdGenerationSetting();
            DefaultRepository = new EmptyRepository();
            IsClearBeforeSeedingEnabled = true;
        }

        public EntitySettingBuilder<T> ForEntity<T>()
        {
            var entityType = typeof (T);
            var newObjectDescriptionBuilder = new EntitySettingBuilder<T>(entityType, new PrimaryKeySetting(DefaultPrimaryKey.PrimaryKeyNames), DefaultRepository, DefaultIdGeneration, _order++);
            _entitySettingBuilders.Add(newObjectDescriptionBuilder);
            return newObjectDescriptionBuilder;
        }

        public void WithDataProvider(IEntityDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        public void AddTypeTransformation(Type type, ITypeTransformation transformation)
        {
            if (TypeTransformations.ContainsKey(type))
            {
                TypeTransformations[type] = transformation;
            }
            else
            {
                TypeTransformations.Add(type, transformation);
            }
        }

        public void WithDefaultPrimaryKeyNames(params string[] primaryKeyNames)
        {
            //todo: is valid primary key (check {} tokens)
            DefaultPrimaryKey = new PrimaryKeySetting(primaryKeyNames.ToList());
        }

        public void WithDefaultRepository(IRepository repository)
        {
            DefaultRepository = repository;
        }

        public void DisableClearBeforeSeeding()
        {
            IsClearBeforeSeedingEnabled = false;
        }

        public void AfterTransformation(Action<Dictionary<string, string>, object> afterTransformationAction)
        {
            AfterTransformationAction = afterTransformationAction;
        }

        public void WithIntegerIdGenerationViaCode(int startId = 1, int steps = 1)
        {
            DefaultIdGeneration = new IdGenerationSetting(new IntegerIdGenerator(startId, steps));
        }

        public void WithGuidIdGenerationViaCode()
        {
            DefaultIdGeneration = new IdGenerationSetting(new GuidIdGenerator());
        }

        public void WithCustomIdGenerationViaCode(IIdGenerator generator)
        {
            DefaultIdGeneration = new IdGenerationSetting(generator);
        }
    }
}