using System.Collections.Generic;
using System.Text;

namespace CherrySeed.DataProviders.Csv
{
    public class CsvDataProviderConfiguration
    {
        public CsvDataProviderConfiguration()
        {
            Delimiter = ";";
            CsvFilePaths = new List<string>();
            Encoding = Encoding.UTF8;
        }

        public string FolderPath { get; set; }
        public List<string> CsvFilePaths { get; set; }
        public string Delimiter { get; set; }
        public Encoding Encoding { get; set; }
    }
}