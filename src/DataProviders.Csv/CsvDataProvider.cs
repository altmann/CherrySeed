using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CherrySeed.EntityDataProvider;

namespace CherrySeed.DataProviders.Csv
{
    public class CsvDataProvider : IDataProvider
    {
        private readonly CsvDataProviderConfiguration _configuration;

        public CsvDataProvider(CsvDataProviderConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<EntityData> GetEntityDataList()
        {
            var csvFilePaths = GetCsvFilePaths();

            return csvFilePaths
                .Select(csvFilePath => new CsvFile(csvFilePath, _configuration.Delimiter, _configuration.Encoding).ReadFile())
                .ToList();
        }

        private IEnumerable<string> GetCsvFilePaths()
        {
            if (_configuration.FolderPath != null)
            {
                return Directory.GetFiles(_configuration.FolderPath, "*.csv").ToList();
            }

            foreach (var csvFilePath in _configuration.CsvFilePaths)
            {
                if(!File.Exists(csvFilePath))
                    throw new InvalidOperationException($"File not found {csvFilePath}");
            }

            return _configuration.CsvFilePaths;
        }
    }
}