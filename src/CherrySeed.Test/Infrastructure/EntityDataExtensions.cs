using System.Collections.Generic;
using CherrySeed.EntityDataProvider;
using CherrySeed.Test.Mocks;

namespace CherrySeed.Test.Infrastructure
{
    public static class EntityDataExtensions
    {
        public static IDataProvider ToDictionaryDataProvider(this List<EntityData> entityDataList)
        {
            return new DictionaryDataProvider(entityDataList);
        }
    }
}