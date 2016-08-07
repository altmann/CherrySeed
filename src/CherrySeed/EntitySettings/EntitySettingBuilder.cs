using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CherrySeed.Repositories;

namespace CherrySeed.EntitySettings
{
    public class EntitySettingBuilder
    {
        protected readonly EntitySetting Obj;

        public EntitySettingBuilder(Type entityType, List<string> defaultPrimaryKeyNames, 
            IRepository defaultRepository, int order)
        {
            Obj = new EntitySetting
            {
                EntityType = entityType,
                Repository = defaultRepository,
                PrimaryKey = new PrimaryKeySetting(defaultPrimaryKeyNames),
                References = new List<ReferenceSetting>(),
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
        public EntitySettingBuilder(Type entityType, List<string> defaultPrimaryKeyNames, IRepository defaultRepository, int order)
            : base(entityType, defaultPrimaryKeyNames, defaultRepository, order)
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
    }
}