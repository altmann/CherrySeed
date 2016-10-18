using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDataProvider;
using CherrySeed.EntitySettings;
using CherrySeed.Test.Mocks;

namespace CherrySeed.Test.Infrastructure
{
    public static class Extensions
    {
        public static EntitySetting GetSetting<T>(this List<EntitySetting> entitySettings)
        {
            var entityType = typeof (T);
            return entitySettings.First(es => es.EntityType == entityType);
        }

        public static Guid ToGuid(this int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static IDataProvider ToDictionaryDataProvider(this List<EntityData> entityDataList)
        {
            return new DictionaryDataProvider(entityDataList);
        }
    }
}