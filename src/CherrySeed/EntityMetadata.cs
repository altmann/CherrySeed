using System;
using System.Collections.Generic;
using CherrySeed.EntitySettings;

namespace CherrySeed
{
    public class EntityMetadata
    {
        //public string EntityName { get; set; }
        public Type EntityType { get; set; }
        public List<Dictionary<string, string>> ObjectsAsDict { get; set; }
        public List<object> Objects { get; set; }
        public EntitySetting EntitySetting { get; set; }
    }
}