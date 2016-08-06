using System.Collections.Generic;

namespace CherrySeed.EntityDefinitions
{
    public class EntityDefinition
    {
        public string EntityName { get; set; }
        public List<Dictionary<string, string>> Objects { get; set; }
    }
}