using System.Collections.Generic;

namespace CherrySeed.EntityDataProvider
{
    public class EntityData
    {
        public string EntityName { get; set; }
        public List<Dictionary<string, string>> Objects { get; set; }
    }
}