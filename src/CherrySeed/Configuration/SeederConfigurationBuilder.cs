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
        IEntitySettingBuilder<T> ForEntity<T>();
        void WithDataProvider(IDataProvider dataProvider);
        void AddTypeTransformation(Type type, ITypeTransformation transformation);
        void WithDefaultPrimaryKeyNames(params string[] primaryKeyNames);
        void WithRepository(IRepository repository);
        void DisableClearBeforeSeeding();
        void AfterTransformation(Action<Dictionary<string, string>, object> afterTransformationAction);
        void WithIntegerIdGenerationViaCode(int startId = 1, int steps = 1);
        void WithGuidIdGenerationViaCode();
        void WithStringIdGenerationViaCode(string prefix = "", int startId = 1, int steps = 1);
        void WithCustomIdGenerationViaCode(IIdGenerator generator);
        void WithEmptyStringMarker(string marker);
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
        public IDataProvider DataProvider { get; set; }
        public Dictionary<Type, ITypeTransformation> TypeTransformations { get; }

        public SeederConfigurationBuilder()
        {
            // init
            _entitySettingBuilders = new List<EntitySettingBuilder>();
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
            DefaultPrimaryKey = new PrimaryKeySetting(new List<string> { "Id", "{ClassName}Id" });
            DefaultIdGeneration = new IdGenerationSetting();
            DefaultRepository = new EmptyRepository();
            IsClearBeforeSeedingEnabled = true;
        }

        public IEntitySettingBuilder<T> ForEntity<T>()
        {
            var entityType = typeof (T);
            var newObjectDescriptionBuilder = new EntitySettingBuilder<T>(entityType, new PrimaryKeySetting(DefaultPrimaryKey.PrimaryKeyNames), DefaultRepository, DefaultIdGeneration, _order++);
            _entitySettingBuilders.Add(newObjectDescriptionBuilder);
            return newObjectDescriptionBuilder;
        }

        public void WithDataProvider(IDataProvider dataProvider)
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

        public void WithRepository(IRepository repository)
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

        public void WithStringIdGenerationViaCode(string prefix = "", int startId = 1, int steps = 1)
        {
            DefaultIdGeneration = new IdGenerationSetting(new StringIdGenerator(prefix, startId, steps));
        }

        public void WithCustomIdGenerationViaCode(IIdGenerator generator)
        {
            DefaultIdGeneration = new IdGenerationSetting(generator);
        }

        public void WithEmptyStringMarker(string marker)
        {
            TypeTransformations[typeof(string)] = new StringTransformation(marker);
        }
    }
}