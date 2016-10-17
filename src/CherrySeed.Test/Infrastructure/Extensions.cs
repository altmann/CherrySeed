using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntitySettings;

namespace CherrySeed.Test.Infrastructure
{
    public static class Extensions
    {
        public static EntitySetting GetSetting<T>(this List<EntitySetting> entitySettings)
        {
            var entityType = typeof (T);
            return entitySettings.First(es => es.EntityType == entityType);
        }
    }
}