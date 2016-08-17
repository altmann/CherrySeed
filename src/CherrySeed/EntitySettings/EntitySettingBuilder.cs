using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CherrySeed.IdGeneration;
using CherrySeed.Repositories;

namespace CherrySeed.EntitySettings
{
    public class EntitySettingBuilder
    {
        protected readonly EntitySetting Obj;

        public EntitySettingBuilder(Type entityType, 
            PrimaryKeySetting primaryKey, 
            IRepository defaultRepository, 
            IdGenerationSetting defaultGenerationSetting, 
            int order)
        {
            Obj = new EntitySetting
            {
                EntityType = entityType,
                Repository = defaultRepository,
                PrimaryKey = primaryKey,
                References = new List<ReferenceSetting>(),
                IdGeneration = defaultGenerationSetting,
                EntityNames = GetFinalEntityNames(entityType),
                Order = order
            };
        }

        private List<string> GetFinalEntityNames(Type entityType)
        {
            var firstEntityName = entityType.FullName;
            var secondEntityName = entityType.Name;

            return new List<string>
            {
                firstEntityName,
                secondEntityName
            };
        }

        public EntitySetting Build()
        {
            return Obj;
        }
    }

    public interface IEntitySettingBuilder<T>
    {
        IEntitySettingBuilder<T> WithPrimaryKey(Expression<Func<T, object>> primaryKeyExpression);
        IEntitySettingBuilder<T> WithReference(Expression<Func<T, object>> referenceExpression, Type referenceEntity);
        IEntitySettingBuilder<T> WithRepository(IRepository repository);
        IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInDatabase();
        IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsInteger(int startId = 1, int steps = 1);
        IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsGuid();
        IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsString(string prefix = "", int startId = 1, int steps = 1);
        IEntitySettingBuilder<T> WithCustomPrimaryKeyIdGenerationInApplication(IIdGenerator generator);
        IEntitySettingBuilder<T> HasEntityName(string entityName);
    }

    public class EntitySettingBuilder<T> : EntitySettingBuilder, IEntitySettingBuilder<T>
    {
        public EntitySettingBuilder(Type entityType, PrimaryKeySetting primaryKey, IRepository defaultRepository, IdGenerationSetting defaultIdGenerationSetting, int order)
            : base(entityType, primaryKey, defaultRepository, defaultIdGenerationSetting, order)
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

        public IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInDatabase()
        {
            Obj.IdGeneration = new IdGenerationSetting(null);
            return this;
        }

        public IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsInteger(int startId = 1, int steps = 1)
        {
            Obj.IdGeneration = new IdGenerationSetting(new IntegerIdGenerator(startId, steps));
            return this;
        }

        public IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsGuid()
        {
            Obj.IdGeneration = new IdGenerationSetting(new GuidIdGenerator());
            return this;
        }

        public IEntitySettingBuilder<T> WithPrimaryKeyIdGenerationInApplicationAsString(string prefix = "", int startId = 1, int steps = 1)
        {
            Obj.IdGeneration = new IdGenerationSetting(new StringIdGenerator(prefix, startId, steps));
            return this;
        }

        public IEntitySettingBuilder<T> WithCustomPrimaryKeyIdGenerationInApplication(IIdGenerator generator)
        {
            Obj.IdGeneration = new IdGenerationSetting(generator);
            return this;
        }

        public IEntitySettingBuilder<T> HasEntityName(string entityName)
        {
            Obj.EntityNames = new List<string> { entityName };
            return this;
        }
    }
}