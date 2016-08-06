using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CherrySeed.Repositories;

namespace CherrySeed.EntitySettings
{
    public class EntitySettingBuilder
    {
        private readonly EntitySetting _obj;

        public EntitySettingBuilder(Type entityType, string defaultPrimaryKeyName, ICreateRepository defaultCreateRepository, IRemoveRepository defaultRemoveRepository, int order)
        {
            _obj = new EntitySetting
            {
                EntityType = entityType,
                CreateRepository = defaultCreateRepository,
                RemoveRepository = defaultRemoveRepository,
                PrimaryKey = new PrimaryKeySetting(defaultPrimaryKeyName),
                References = new List<ReferenceSetting>(),
                Order = order
            };
        }

        public EntitySettingBuilder WithPrimaryKey<T>(Expression<Func<T, object>> primaryKeyExpression)
        {
            _obj.PrimaryKey = new PrimaryKeySetting<T>(primaryKeyExpression);
            return this;
        }

        public EntitySettingBuilder WithReference<T>(Expression<Func<T, object>> referenceExpression,
            Type referenceEntity)
        {
            _obj.References.Add(new ReferenceSetting<T>(referenceExpression, referenceEntity));
            return this;
        }

        public EntitySettingBuilder WithCreateEntityTarget(ICreateRepository createRepository)
        {
            _obj.CreateRepository = createRepository;
            return this;
        }

        public EntitySettingBuilder WithRemoveEntitiesTarget(IRemoveRepository removeRepository)
        {
            _obj.RemoveRepository = removeRepository;
            return this;
        }

        public EntitySetting Build()
        {
            return _obj;
        }
    }
}