using System;
using System.Collections.Generic;
using CherrySeed.IdGeneration;
using CherrySeed.Repositories;

namespace CherrySeed.EntitySettings
{
    public enum IdGenerationType
    {
        IdGenerationViaDatabase,
        IntegerIdGenerationViaCode,
        GuidIdGenerationViaCode
    }

    public class IdGenerationSetting
    {
        public IdGenerationSetting()
        { }

        public IdGenerationSetting(IIdGenerator generator)
        {
            Generator = generator;
        }

        public IIdGenerator Generator { get; set; }
    }

    public class CompositeEntitySettingBuilder
    {
        public List<EntitySettingBuilder> ObjectDescriptionBuilders { get; }
        public List<string> DefaultPrimaryKeyNames { get; set; }
        public IRepository DefaultRepository { get; set; }
        public IdGenerationSetting DefaultIdGeneration { get; set; }
        private int _order = 1;

        public CompositeEntitySettingBuilder()
        {
            ObjectDescriptionBuilders = new List<EntitySettingBuilder>();

            DefaultPrimaryKeyNames = new List<string> {"Id", "{ClassName}Id"};

            var emptyTarget = new EmptyRepository();
            DefaultRepository = emptyTarget;

            DefaultIdGeneration = new IdGenerationSetting(null);
        }

        public EntitySettingBuilder<T> ForEntity<T>()
        {
            var entityType = typeof (T);
            var newObjectDescriptionBuilder = new EntitySettingBuilder<T>(entityType, DefaultPrimaryKeyNames, DefaultRepository, DefaultIdGeneration, _order++);
            ObjectDescriptionBuilders.Add(newObjectDescriptionBuilder);
            return newObjectDescriptionBuilder;
        }
    }
}