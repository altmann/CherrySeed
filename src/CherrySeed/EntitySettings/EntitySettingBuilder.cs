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
                Order = order
            };
        }

        //todo: make it public invisible
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
        IEntitySettingBuilder<T> WithIdGenerationViaDatabase();
        IEntitySettingBuilder<T> WithIntegerIdGenerationViaCode(int startId = 1, int steps = 1);
        IEntitySettingBuilder<T> WithGuidIdGenerationViaCode();
        IEntitySettingBuilder<T> WithCustomIdGenerationViaCode(IIdGenerator generator);
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

        public IEntitySettingBuilder<T> WithReference(Expression<Func<T, object>> referenceExpression,
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

        public IEntitySettingBuilder<T> WithIdGenerationViaDatabase()
        {
            Obj.IdGeneration = new IdGenerationSetting(null);
            return this;
        }

        public IEntitySettingBuilder<T> WithIntegerIdGenerationViaCode(int startId = 1, int steps = 1)
        {
            Obj.IdGeneration = new IdGenerationSetting(new IntegerIdGenerator(startId, steps));
            return this;
        }

        public IEntitySettingBuilder<T> WithGuidIdGenerationViaCode()
        {
            Obj.IdGeneration = new IdGenerationSetting(new GuidIdGenerator());
            return this;
        }

        public IEntitySettingBuilder<T> WithCustomIdGenerationViaCode(IIdGenerator generator)
        {
            Obj.IdGeneration = new IdGenerationSetting(generator);
            return this;
        }
    }
}