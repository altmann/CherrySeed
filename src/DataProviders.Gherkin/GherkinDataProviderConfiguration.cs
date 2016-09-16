using System.Collections.Generic;

namespace CherrySeed.DataProviders.Gherkin
{
    public class GherkinDataProviderConfiguration
    {
        public GherkinDataProviderConfiguration()
        {
            FilePaths = new List<string>();
        }

        public List<string> FilePaths { get; set; }
    }
}