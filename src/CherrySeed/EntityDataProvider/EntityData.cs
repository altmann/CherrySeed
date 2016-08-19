using System.Collections.Generic;

namespace CherrySeed.EntityDataProvider
{
    public class EntityData
    {
        public EntityData()
        {
            Objects = new List<Dictionary<string, string>>();
        }

        public string EntityName { get; set; }
        public List<Dictionary<string, string>> Objects { get; set; }
    }
}