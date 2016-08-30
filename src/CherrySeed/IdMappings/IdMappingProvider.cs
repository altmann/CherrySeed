using System;
using System.Collections.Generic;
using System.Linq;

namespace CherrySeed.IdMappings
{
    class IdMappingProvider
    {
        private readonly Dictionary<Type, List<IdMappingDescription>> _idMappingDict;

        public IdMappingProvider()
        {
            _idMappingDict = new Dictionary<Type, List<IdMappingDescription>>();
        }

        public void SetIdMapping(Type objectType, string providerId, object repositoryId)
        {
            var idMapping = new IdMappingDescription {ProviderId = providerId, RepositoryId = repositoryId};

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

        public object GetRepositoryId(Type objectType, string providerId)
        {
            return _idMappingDict[objectType].Where(idMapping => idMapping.ProviderId == providerId).Select(idMapping => idMapping.RepositoryId).First();
        }
    }
}