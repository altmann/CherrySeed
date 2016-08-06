using System;
using System.Collections.Generic;
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
}