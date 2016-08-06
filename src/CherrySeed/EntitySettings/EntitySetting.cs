using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using CherrySeed.Repositories;

namespace CherrySeed.EntitySettings
{
    public class EntitySetting
    {
        public Type EntityType { get; set; }
        public PrimaryKeySetting PrimaryKey { get; set; }
        public List<ReferenceSetting> References { get; set; }
        public ICreateRepository CreateRepository { get; set; }
        public IRemoveRepository RemoveRepository { get; set; }
        public int Order { get; set; }
    }

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

    public class CompositeEntitySettingBuilder
    {
        public List<EntitySettingBuilder> ObjectDescriptionBuilders { get; }
        public string DefaultPrimaryKeyName { get; set; }
        public ICreateRepository DefaultCreateRepository { get; set; }
        public IRemoveRepository DefaultRemoveRepository { get; set; }

        private int _order = 1;

        public CompositeEntitySettingBuilder()
        {
            ObjectDescriptionBuilders = new List<EntitySettingBuilder>();

            var emptyTarget = new EmptyTarget();

            DefaultPrimaryKeyName = "Id";
            DefaultCreateRepository = emptyTarget;
            DefaultRemoveRepository = emptyTarget;
        }

        public EntitySettingBuilder ForEntity(Type entityType)
        {
            var newObjectDescriptionBuilder = new EntitySettingBuilder(entityType, DefaultPrimaryKeyName, DefaultCreateRepository, DefaultRemoveRepository, _order++);
            ObjectDescriptionBuilders.Add(newObjectDescriptionBuilder);
            return newObjectDescriptionBuilder;
        }
    }
}