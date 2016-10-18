using CherrySeed.DataProviders.SpecFlow.Test.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.DataProviders.SpecFlow.Test.IntegrationTests
{
    public static class EntityAsserts
    {
        public static void AssertCountry(Country actual, Country expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        public static void AssertProject(Project actual, Project expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.CountryId, actual.CountryId);
        }
    }
}