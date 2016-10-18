using System;
using TechTalk.SpecFlow;

namespace CherrySeed.DataProviders.SpecFlow.Test.IntegrationTests
{
    public class ObjectMother
    {
        public static Table CreateCountryTable(Action<Table> addDataFunc)
        {
            var table = new Table("Id", "Name");
            addDataFunc(table);
            return table;
        }

        public static Table CreateProjectTable(Action<Table> addDataFunc)
        {
            var table = new Table("Id", "Name", "CountryId");
            addDataFunc(table);
            return table;
        }
    }
}