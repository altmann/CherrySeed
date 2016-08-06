using System.Collections.Generic;

namespace CherrySeed.EntityDefinitions
{
    public class DefaultEntityDefinitionProvider : IEntityDefinitionProvider
    {
        private readonly List<EntityDefinition> _objectDefinitions;

        public DefaultEntityDefinitionProvider(List<EntityDefinition> objectDefinitions)
        {
            _objectDefinitions = objectDefinitions;
        }

        public List<EntityDefinition> GetEntityDefinitions()
        {
            return _objectDefinitions;
        }
    }
    
}