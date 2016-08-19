using System.Collections.Generic;
using CherrySeed.DataProviders.Csv;
using DataProviders.Csv.Test.Asserts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataProviders.Csv.Test
{
    [TestClass]
    public class CsvDataProviderTest
    {
        [TestMethod]
        public void ReadCsvsFromDictionary()
        {
            var csvDataProvider = new CsvDataProvider("./CsvFiles");
            var entityDataList = csvDataProvider.GetEntityDataList();

            Assert.IsNotNull(entityDataList);

            var entityDataComplex = entityDataList[0];
            MyAssert.AssertComplex(entityDataComplex);

            var entityDataSimple = entityDataList[1];
            MyAssert.AssertSimple(entityDataSimple);
        }

        [TestMethod]
        public void ReadSimpleCsvFile()
        {
            var csvDataProvider = new CsvDataProvider(new List<string> { "./CsvFiles/Simple.csv" });
            var entityDataList = csvDataProvider.GetEntityDataList();

            Assert.IsNotNull(entityDataList);

            var entityDataSimple = entityDataList[0];
            MyAssert.AssertSimple(entityDataSimple);
        }

        [TestMethod]
        public void ReadSimpleEmptyCsvFile()
        {
            var csvDataProvider = new CsvDataProvider(new List<string> { "./CsvFiles/SimpleEmpty.csv" });
            var entityDataList = csvDataProvider.GetEntityDataList();

            Assert.IsNotNull(entityDataList);

            var entityDataSimple = entityDataList[0];
            MyAssert.AssertSimpleEmpty(entityDataSimple);
        }
    }
}
