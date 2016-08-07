using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CherrySeed.Repositories;

namespace CherrySeed.EntitySettings
{
    public class EntitySettingBuilder
    {
        protected readonly EntitySetting Obj;

        public EntitySettingBuilder(Type entityType, List<string> defaultPrimaryKeyNames, ICreateRepository defaultCreateRepository, IRemoveRepository defaultRemoveRepository, int order)
        {
            Obj = new EntitySetting
            {
                EntityType = entityType,
                CreateRepository = defaultCreateRepository,
                RemoveRepository = defaultRemoveRepository,
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
        public EntitySettingBuilder(Type entityType, List<string> defaultPrimaryKeyNames, ICreateRepository defaultCreateRepository, IRemoveRepository defaultRemoveRepository, int order)
            : base(entityType, defaultPrimaryKeyNames, defaultCreateRepository, defaultRemoveRepository, order)
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

        public EntitySettingBuilder<T> WithCreateEntityTarget(ICreateRepository createRepository)
        {
            Obj.CreateRepository = createRepository;
            return this;
        }

        public EntitySettingBuilder<T> WithRemoveEntitiesTarget(IRemoveRepository removeRepository)
        {
            Obj.RemoveRepository = removeRepository;
            return this;
        }
    }
}