using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDataProvider;
using TechTalk.SpecFlow;

namespace CherrySeed.DataProviders.SpecFlow
{
    class SpecFlowDataProvider : IDataProvider
    {
        private List<EntityData> _entityDataList;

        public void ClearAndAdd(string entityName, Table table)
        {
            _entityDataList = TransformTableToEntityData(entityName, table);
        }

        private List<EntityData> TransformTableToEntityData(string entityName, Table table)
        {
            return new List<EntityData>
            {
                new EntityData
                {
                    EntityName = entityName,
                    Objects = table.Rows
                    .Select(r => r.ToDictionary(
                        instance => instance.Key, 
                        instance => instance.Value)
                        ).ToList()
                }
            };
        }

        public List<EntityData> GetEntityDataList()
        {
            return _entityDataList;
        }
    }
}