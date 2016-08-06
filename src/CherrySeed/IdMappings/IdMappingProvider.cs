using System;
using System.Collections.Generic;
using System.Linq;

namespace CherrySeed.IdMappings
{
    public class IdMappingProvider
    {
        private readonly Dictionary<Type, List<IdMappingDescription>> _idMappingDict;


        public IdMappingProvider()
        {
            _idMappingDict = new Dictionary<Type, List<IdMappingDescription>>();
        }

        public void SetIdMapping(Type objectType, string definitionId, object targetId)
        {
            var idMapping = new IdMappingDescription {DefinitionId = definitionId, TargetId = targetId};

            if (_idMappingDict.ContainsKey(objectType))
            {
                _idMappingDict[objectType].Add(idMapping);
            }
            else
            {
                _idMappingDict.Add(objectType, new List<IdMappingDescription>
                {
                    idMapping
                });
            } 
        }

        public object GetTargetId(Type objectType, string definitionId)
        {
            return _idMappingDict[objectType].Where(idMapping => idMapping.DefinitionId == definitionId).Select(idMapping => idMapping.TargetId).First();
        }
    }

    public class IdMappingDescription
    {
        public string DefinitionId { get; set; }
        public object TargetId { get; set; }
    }
}