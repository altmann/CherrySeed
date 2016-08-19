using System.Collections.Generic;
using CherrySeed.EntityDataProvider;

namespace CherrySeed.Test.Mocks
{
    public class DictionaryDataProvider : IDataProvider
    {
        private readonly List<EntityData> _entityData;

        public DictionaryDataProvider(List<EntityData> entityData)
        {
            _entityData = entityData;
        }

        public List<EntityData> GetEntityDataList()
        {
            return _entityData;
        }
    }
}