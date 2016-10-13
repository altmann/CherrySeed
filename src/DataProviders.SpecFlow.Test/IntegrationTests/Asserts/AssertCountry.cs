using CherrySeed.DataProviders.SpecFlow.Test.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.DataProviders.SpecFlow.Test.IntegrationTests.Asserts
{
    public static class AssertCountry
    {
        public static void AssertProperties(object actual, Country expected)
        {
            var actualCountry = actual as Country;

            Assert.IsNotNull(actualCountry);
            Assert.AreEqual(actualCountry.Id, expected.Id);
            Assert.AreEqual(actualCountry.Name, expected.Name);
        }
    }
}