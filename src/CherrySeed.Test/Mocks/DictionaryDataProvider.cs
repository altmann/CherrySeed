using System.Collections.Generic;
using CherrySeed.EntityDataProvider;

namespace CherrySeed.Test.Mocks
{
    public class DictionaryDataProvider : IEntityDataProvider
    {
        private readonly List<EntityData> _entityData;

        public DictionaryDataProvider(List<EntityData> entityData)
        {
            _entityData = entityData;
        }

        public List<EntityData> GetEntityData()
        {
            return _entityData;
        }
    }

    public static class CherrySeederExtension
    {


        public static void UseDictionaryDataProvider(this CherrySeeder cherrySeeder,
            List<EntityData> entityData)
        {
            cherrySeeder.EntityDataProvider = new DictionaryDataProvider(entityData);
        }
    }
}