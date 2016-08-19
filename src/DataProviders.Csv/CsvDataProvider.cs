using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CherrySeed.EntityDataProvider;

namespace CherrySeed.DataProviders.Csv
{
    public class CsvDataProvider : IDataProvider
    {
        private readonly List<string> _csvFilePaths;
        private readonly string _folderPath;
        private readonly string _delimiter;
        private readonly Encoding _encoding;

        public CsvDataProvider(string folderPath, string delimiter = ";")
        {
            _folderPath = folderPath;
            _delimiter = delimiter;
            _encoding = new UTF8Encoding();
        }

        public CsvDataProvider(List<string> csvFilePaths, string delimiter = ";")
        {
            _csvFilePaths = csvFilePaths;
            _delimiter = delimiter;
            _encoding = new UTF8Encoding();
        }

        public List<EntityData> GetEntityDataList()
        {
            var csvFilePaths = GetCsvFilePaths();

            return csvFilePaths
                .Select(csvFilePath => new CsvFile(csvFilePath, _delimiter, _encoding).ReadFile())
                .ToList();
        }

        private IEnumerable<string> GetCsvFilePaths()
        {
            if (_folderPath != null)
            {
                return Directory.GetFiles(_folderPath, "*.csv").ToList();
            }

            foreach (var csvFilePath in _csvFilePaths)
            {
                if(!File.Exists(csvFilePath))
                    throw new InvalidOperationException($"File not found {csvFilePath}");
            }

            return _csvFilePaths;
        }
    }
}