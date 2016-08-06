using System;
using System.Collections.Generic;
using CherrySeed.Repositories;

namespace CherrySeed.EntitySettings
{
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

            var emptyTarget = new EmptyRepository();

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