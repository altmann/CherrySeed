using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDataProvider;
using Gherkin;
using Gherkin.Ast;

namespace CherrySeed.DataProviders.Gherkin
{
    public class GherkinDataProvider : IDataProvider
    {
        private readonly GherkinDataProviderConfiguration _config;

        public GherkinDataProvider(GherkinDataProviderConfiguration config)
        {
            _config = config;
        }

        public List<EntityData> GetEntityDataList()
        {
            var entities = new List<EntityData>();

            foreach (var filePath in _config.FilePaths)
            {
                var parser = new Parser();
                var gherkinDocument = parser.Parse(filePath);

                foreach (var scenarioDefinition in gherkinDocument.Feature.Children)
                {
                    foreach (var step in scenarioDefinition.Steps)
                    {
                        var entityName = step.Text.Split('\'')[1];
                        var dataTable = step.Argument as DataTable;
                        var entityDicts = new List<Dictionary<string, string>>();

                        foreach (var row in dataTable.Rows.Skip(1))
                        {
                            var entityDict = TransformEntity(row, dataTable);
                            entityDicts.Add(entityDict);
                        }

                        entities.Add(new EntityData
                        {
                            EntityName = entityName,
                            Objects = entityDicts
                        });
                    }
                }
            }

            return entities;
        }

        private static Dictionary<string, string> TransformEntity(TableRow row, DataTable dataTable)
        {
            var entityDict = new Dictionary<string, string>();

            for (var i = 0; i < row.Cells.Count(); i++)
            {
                var cell = row.Cells.ToList()[i];
                entityDict.Add(dataTable.Rows.First().Cells.ToList()[i].Value, cell.Value);
            }
            return entityDict;
        }
    }
}