using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CherrySeed.EntityTargets;

namespace CherrySeed.EntitySettings
{
    public class EntitySetting
    {
        public Type EntityType { get; set; }
        public PrimaryKeySetting PrimaryKey { get; set; }
        public List<ReferenceSetting> References { get; set; }
        public ICreateEntityTarget CreateEntityTarget { get; set; }
        public IRemoveEntitiesTarget RemoveEntitiesTarget { get; set; }
        public int Order { get; set; }
    }

    public class EntitySettingBuilder
    {
        private readonly EntitySetting _obj;

        public EntitySettingBuilder(Type entityType, string defaultPrimaryKeyName, ICreateEntityTarget defaultCreateEntityTarget, IRemoveEntitiesTarget defaultRemoveEntitiesTarget, int order)
        {
            _obj = new EntitySetting
            {
                EntityType = entityType,
                CreateEntityTarget = defaultCreateEntityTarget,
                RemoveEntitiesTarget = defaultRemoveEntitiesTarget,
                PrimaryKey = new PrimaryKeySetting(defaultPrimaryKeyName),
                References = new List<ReferenceSetting>(),
                Order = order
            };
        }

        public EntitySettingBuilder WithPrimaryKey(Expression<Func<object, object>> primaryKeyExpression)
        {
            _obj.PrimaryKey = new PrimaryKeySetting(primaryKeyExpression);
            return this;
        }

        public EntitySettingBuilder WithReference(Expression<Func<object, object>> referenceExpression,
            Type referenceEntity)
        {
            _obj.References.Add(new ReferenceSetting(referenceExpression, referenceEntity));
            return this;
        }

        public EntitySettingBuilder WithCreateEntityTarget(ICreateEntityTarget createEntityTarget)
        {
            _obj.CreateEntityTarget = createEntityTarget;
            return this;
        }

        public EntitySettingBuilder WithRemoveEntitiesTarget(IRemoveEntitiesTarget removeEntitiesTarget)
        {
            _obj.RemoveEntitiesTarget = removeEntitiesTarget;
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
        public ICreateEntityTarget DefaultCreateEntityTarget { get; set; }
        public IRemoveEntitiesTarget DefaultRemoveEntitiesTarget { get; set; }

        private int _order = 1;

        public CompositeEntitySettingBuilder()
        {
            ObjectDescriptionBuilders = new List<EntitySettingBuilder>();

            var emptyTarget = new EmptyTarget();

            DefaultPrimaryKeyName = "Id";
            DefaultCreateEntityTarget = emptyTarget;
            DefaultRemoveEntitiesTarget = emptyTarget;
        }

        public EntitySettingBuilder ForEntity(Type entityType)
        {
            var newObjectDescriptionBuilder = new EntitySettingBuilder(entityType, DefaultPrimaryKeyName, DefaultCreateEntityTarget, DefaultRemoveEntitiesTarget, _order++);
            ObjectDescriptionBuilders.Add(newObjectDescriptionBuilder);
            return newObjectDescriptionBuilder;
        }
    }
}