using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CherrySeed.Repositories;

namespace CherrySeed.EntitySettings
{
    public class EntitySettingBuilder
    {
        protected readonly EntitySetting Obj;
        protected CodeIdGenerationSettingBuilder CodeIdGenerationSettingBuilder;

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

        public EntitySettingBuilder<T> WithDatabaseIdGeneration()
        {
            Obj.IdGeneration = new IdGenerationSetting(null);
            return this;
        }

        public CodeIdGenerationSettingBuilder WithCodeIdGeneration()
        {
            CodeIdGenerationSettingBuilder = new CodeIdGenerationSettingBuilder();
            return CodeIdGenerationSettingBuilder;
        }
    }
}