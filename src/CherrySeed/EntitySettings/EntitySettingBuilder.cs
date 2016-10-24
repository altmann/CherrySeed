using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CherrySeed.DefaultValues;
using CherrySeed.PrimaryKeyIdGeneration;
using CherrySeed.PrimaryKeyIdGeneration.ApplicationGeneration;
using CherrySeed.Repositories;
using CherrySeed.Utils;

namespace CherrySeed.EntitySettings
{
    public class EntitySettingBuilder
    {
        protected readonly EntitySetting Obj;

        public EntitySettingBuilder(Type entityType, int order)
        {
            Obj = new EntitySetting
            {
                EntityType = entityType,
                PrimaryKey = new PrimaryKeySetting(),
                References = new List<ReferenceSetting>(),
                IdGeneration = null,
                EntityNames = GetPossibleEntityNames(entityType),
                AfterSave = obj => { },
                DefaultValueSettings = new List<DefaultValueSetting>(),
                Order = order
            };
        }

        private List<string> GetPossibleEntityNames(Type entityType)
        {
            var firstEntityName = entityType.FullName;
            var secondEntityName = entityType.Name;

            return new List<string>
            {
                firstEntityName,
                secondEntityName
            };
        }

        public EntitySetting Build(IRepository defaultRepository, IdGenerationSetting defaultGenerationSetting, List<string> defaultPrimaryKeyNames)
        {
            if (Obj.Repository == null)
            {
                Obj.Repository = defaultRepository;
            }

            if (Obj.IdGeneration == null)
            {
                Obj.IdGeneration = defaultGenerationSetting;
            }

            if (string.IsNullOrEmpty(Obj.PrimaryKey.PrimaryKeyName))
            {
                Obj.PrimaryKey.PrimaryKeyName = GetFinalPrimaryKeyName(Obj.EntityType, defaultPrimaryKeyNames);
            }

            return Obj;
        }

        private string GetFinalPrimaryKeyName(Type type, List<string> defaultPrimaryKeyNames)
        {
            return defaultPrimaryKeyNames
                .Select(propertyName => propertyName.Replace("{ClassName}", type.Name))
                .FirstOrDefault(propertyNameWithoutTokens => ReflectionUtil.ExistProperty(type, propertyNameWithoutTokens));
        }
    }

    public interface IEntitySettingBuilder<T>
    {
        IEntitySettingBuilder<T> WithPrimaryKey(Expression<Func<T, object>> primaryKeyExpression);
        IEntitySettingBuilder<T> WithReference(Expression<Func<T, object>> referenceExpression, Type referenceEntity);
        IEntitySettingBuilder<T> WithRepository(IRepository repository);
        IEntitySettingBuilder<T> WithDisabledPrimaryKeyIdGeneration();
        IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInDatabase();
        IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsInteger(int startId = 1, int steps = 1);
        IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsGuid();
        IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsString(string prefix = "", int startId = 1, int steps = 1);
        IEntitySettingBuilder<T> WithCustomPrimaryKeyIdGenerationInApplication(IPrimaryKeyIdGenerator generator);
        IEntitySettingBuilder<T> HasEntityName(string entityName);
        IEntitySettingBuilder<T> AfterSave(Action<object> afterSaveAction);
        IEntitySettingBuilder<T> WithFieldWithDefaultValue(Expression<Func<T, object>> fieldExpression,
            IDefaultValueProvider defaultValueProvider);
        IEntitySettingBuilder<T> WithFieldWithDefaultValue(Expression<Func<T, object>> fieldExpression,
            Func<object> defaultValueFunc);
    }

    public class EntitySettingBuilder<T> : EntitySettingBuilder, IEntitySettingBuilder<T>
    {
        public EntitySettingBuilder(Type entityType, int order)
            : base(entityType, order)
        { }

        public IEntitySettingBuilder<T> WithPrimaryKey(Expression<Func<T, object>> primaryKeyExpression)
        {
            Obj.PrimaryKey = new PrimaryKeySetting<T>(primaryKeyExpression);
            return this;
        }

        public IEntitySettingBuilder<T> WithReference(
            Expression<Func<T, object>> referenceExpression,
            Type referenceEntity)
        {
            Obj.References.Add(new ReferenceSetting<T>(referenceExpression, referenceEntity));
            return this;
        }

        public IEntitySettingBuilder<T> WithRepository(IRepository repository)
        {
            Obj.Repository = repository;
            return this;
        }

        public IEntitySettingBuilder<T> WithDisabledPrimaryKeyIdGeneration()
        {
            Obj.IdGeneration = new IdGenerationSetting
            {
                Generator = null
            };
            return this;
        }

        public IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInDatabase()
        {
            Obj.IdGeneration = new IdGenerationSetting
            {
                Generator = new DatabasePrimaryKeyIdGeneration()
            };
            return this;
        }

        public IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsInteger(int startId = 1, int steps = 1)
        {
            Obj.IdGeneration = new IdGenerationSetting
            {
                Generator = new ApplicationPrimaryKeyIdGeneration
                {
                    Generator = new IntegerPrimaryKeyIdGenerator(startId, steps)
                }
            };
            return this;
        }

        public IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsGuid()
        {
            Obj.IdGeneration = new IdGenerationSetting
            {
                Generator = new ApplicationPrimaryKeyIdGeneration
                {
                    Generator = new GuidPrimaryKeyIdGenerator()
                }
            };
            return this;
        }

        public IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsString(string prefix = "", int startId = 1, int steps = 1)
        {
            Obj.IdGeneration = new IdGenerationSetting
            {
                Generator = new ApplicationPrimaryKeyIdGeneration
                {
                    Generator = new StringPrimaryKeyIdGenerator(prefix, startId, steps)
                }
            };
            return this;
        }

        public IEntitySettingBuilder<T> WithCustomPrimaryKeyIdGenerationInApplication(IPrimaryKeyIdGenerator generator)
        {
            Obj.IdGeneration = new IdGenerationSetting
            {
                Generator = new ApplicationPrimaryKeyIdGeneration
                {
                    Generator = generator
                }
            };
            return this;
        }

        public IEntitySettingBuilder<T> HasEntityName(string entityName)
        {
            Obj.EntityNames = new List<string> { entityName };
            return this;
        }

        public IEntitySettingBuilder<T> AfterSave(Action<object> afterSaveAction)
        {
            Obj.AfterSave = afterSaveAction;
            return this;
        }

        public IEntitySettingBuilder<T> WithFieldWithDefaultValue(Expression<Func<T, object>> fieldExpression, IDefaultValueProvider defaultValueProvider)
        {
            Obj.DefaultValueSettings.Add(new DefaultValueSetting<T>(fieldExpression, defaultValueProvider));
            return this;
        }

        public IEntitySettingBuilder<T> WithFieldWithDefaultValue(Expression<Func<T, object>> fieldExpression, Func<object> defaultValueFunc)
        {
            Obj.DefaultValueSettings.Add(new DefaultValueSetting<T>(fieldExpression, new FuncDefaultValueProvider(defaultValueFunc)));
            return this;
        }
    }
}