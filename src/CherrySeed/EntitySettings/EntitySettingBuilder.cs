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

        public EntitySettingBuilder(Type entityType, List<string> defaultPrimaryKeyNames, 
            IRepository defaultRepository, IdGenerationSetting defaultGenerationSetting, int order)
        {
            Obj = new EntitySetting
            {
                EntityType = entityType,
                Repository = defaultRepository,
                PrimaryKey = new PrimaryKeySetting(defaultPrimaryKeyNames),
                References = new List<ReferenceSetting>(),
                IdGeneration = defaultGenerationSetting,
                Order = order
            };
        }

        public EntitySetting Build()
        {
            return Obj;
        }
    }

    public class EntitySettingBuilder<T> : EntitySettingBuilder
    {
        public EntitySettingBuilder(Type entityType, List<string> defaultPrimaryKeyNames, IRepository defaultRepository, IdGenerationSetting defaultIdGenerationSetting, int order)
            : base(entityType, defaultPrimaryKeyNames, defaultRepository, defaultIdGenerationSetting, order)
        { }

        public EntitySettingBuilder<T> WithPrimaryKey(Expression<Func<T, object>> primaryKeyExpression)
        {
            Obj.PrimaryKey = new PrimaryKeySetting<T>(primaryKeyExpression);
            return this;
        }

        public EntitySettingBuilder<T> WithReference(Expression<Func<T, object>> referenceExpression,
            Type referenceEntity)
        {
            Obj.References.Add(new ReferenceSetting<T>(referenceExpression, referenceEntity));
            return this;
        }

        public EntitySettingBuilder<T> WithRepository(IRepository repository)
        {
            Obj.Repository = repository;
            return this;
        }

        public EntitySettingBuilder<T> WithIdGenerationViaDatabase()
        {
            Obj.IdGeneration = new IdGenerationSetting(null);
            return this;
        }

        public EntitySettingBuilder<T> WithIntegerIdGenerationViaCode(int startId = 1, int steps = 1)
        {
            Obj.IdGeneration = new IdGenerationSetting(new IntegerIdGenerator(startId, steps));
            return this;
        }

        public EntitySettingBuilder<T> WithGuidIdGenerationViaCode()
        {
            Obj.IdGeneration = new IdGenerationSetting(new GuidIdGenerator());
            return this;
        }

        public EntitySettingBuilder<T> WithCustomIdGenerationViaCode(IIdGenerator generator)
        {
            Obj.IdGeneration = new IdGenerationSetting(generator);
            return this;
        }
    }
}