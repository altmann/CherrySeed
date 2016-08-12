using System;
using System.Collections.Generic;
using CherrySeed.Configuration;
using CherrySeed.Repositories;

namespace CherrySeed.EntitySettings
{
    public class EntitySetting
    {
        public Type EntityType { get; set; }
        public PrimaryKeySetting PrimaryKey { get; set; }
        public List<ReferenceSetting> References { get; set; }
        public IRepository Repository { get; set; }
        public int Order { get; set; }
        public IdGenerationSetting IdGeneration { get; set; }
    }
}