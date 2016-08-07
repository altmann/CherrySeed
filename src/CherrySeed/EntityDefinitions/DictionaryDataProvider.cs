using System.Collections.Generic;

namespace CherrySeed.EntityDefinitions
{
    public class DictionaryDataProvider : IEntityDefinitionProvider
    {
        private readonly List<EntityDefinition> _objectDefinitions;

        public DictionaryDataProvider(List<EntityDefinition> objectDefinitions)
        {
            _objectDefinitions = objectDefinitions;
        }

        public List<EntityDefinition> GetEntityDefinitions()
        {
            return _objectDefinitions;
        }
    }

    public static class CherrySeederExtension
    {
        public static void UseDictionaryDataProvider(this CherrySeeder cherrySeeder,
            List<EntityDefinition> entityData)
        {
            cherrySeeder.EntityDefinitionProvider = new DictionaryDataProvider(entityData);
        }
    }
}