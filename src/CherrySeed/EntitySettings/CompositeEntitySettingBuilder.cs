using System;
using System.Collections.Generic;
using CherrySeed.Repositories;

namespace CherrySeed.EntitySettings
{
    public class CompositeEntitySettingBuilder
    {
        public List<EntitySettingBuilder> ObjectDescriptionBuilders { get; }
        public List<string> DefaultPrimaryKeyNames { get; set; }
        public IRepository DefaultRepository { get; set; }
        private int _order = 1;

        public CompositeEntitySettingBuilder()
        {
            ObjectDescriptionBuilders = new List<EntitySettingBuilder>();

            var emptyTarget = new EmptyRepository();

            DefaultPrimaryKeyNames = new List<string> {"Id", "{ClassName}Id"};

            DefaultRepository = emptyTarget;
        }

        public EntitySettingBuilder<T> ForEntity<T>()
        {
            var entityType = typeof (T);
            var newObjectDescriptionBuilder = new EntitySettingBuilder<T>(entityType, DefaultPrimaryKeyNames, DefaultRepository, _order++);
            ObjectDescriptionBuilders.Add(newObjectDescriptionBuilder);
            return newObjectDescriptionBuilder;
        }
    }
}