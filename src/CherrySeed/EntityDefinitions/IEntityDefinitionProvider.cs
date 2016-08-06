using System.Collections.Generic;

namespace CherrySeed.EntityDefinitions
{
    public interface IEntityDefinitionProvider
    {
        List<EntityDefinition> GetEntityDefinitions();
    }
}