using System.Collections.Generic;
using CherrySeed.EntityDataProvider;

namespace CherrySeed.Test.Infrastructure
{
    public class EntityDataBuilder
    {
        private readonly string _entityName;
        private readonly string[] _propertyNames;
        private readonly List<string[]> _entities; 

        public EntityDataBuilder(string entityName, params string[] propertyNames)
        {
            _entityName = entityName;
            _propertyNames = propertyNames;
            _entities = new List<string[]>();
        }

        public EntityDataBuilder WithEntity(params string[] data)
        {
            _entities.Add(data);
            return this;
        }

        public EntityData Build()
        {
            return new EntityData
            {
                EntityName = _entityName,
                Objects = CreateObjectDictionaryList()
            };
        }

        private List<Dictionary<string, string>> CreateObjectDictionaryList()
        {
            var result = new List<Dictionary<string, string>>();

            foreach (var entityStringArray in _entities)
            {
                var objectDict = new Dictionary<string, string>();

                for (int i = 0; i < _propertyNames.Length; i++)
                {
                    objectDict.Add(_propertyNames[i], entityStringArray[i]);
                }

                result.Add(objectDict);
            }

            return result;
        }
    }
}