using CherrySeed.DataProviders.SpecFlow.Test.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.DataProviders.SpecFlow.Test.IntegrationTests.Asserts
{
    public static class AssertProject
    {
        public static void AssertProperties(object actual, Project expected)
        {
            var actualProject = actual as Project;

            Assert.IsNotNull(actualProject);
            Assert.AreEqual(actualProject.Id, expected.Id);
            Assert.AreEqual(actualProject.Name, expected.Name);
            Assert.AreEqual(actualProject.CountryId, expected.CountryId);
        }
    }

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