using CherrySeed.Configuration;
using CherrySeed.DataProviders.SpecFlow.Test.Common;
using CherrySeed.DataProviders.SpecFlow.Test.Entities;
using CherrySeed.DataProviders.SpecFlow.Test.IntegrationTests.Asserts;
using CherrySeed.DataProviders.SpecFlow.Test.IntegrationTests.ObjectMothers;
using CherrySeed.Test.Base.Asserts;
using CherrySeed.Test.Base.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.DataProviders.SpecFlow.Test.IntegrationTests
{
    [TestClass]
    public class SpecFlowExtensionsTests
    {
        [TestMethod]
        public void SeedCountryAndProject_SeededSuccessfully()
        {
            var countryTable = ObjectMother.CreateCountryTable(table =>
            {
                table.AddRow("C1", "Austria");
                table.AddRow("C2", "Germany");
            });

            var projectTable = ObjectMother.CreateProjectTable(table =>
            {
                table.AddRow("P1", "Project1", "C1");
                table.AddRow("P2", "Project2", "C2");
            });

            var assertRepository = new AssertRepository((obj, count, repo) =>
            {
                AssertHelper.AssertIf(typeof(Country), 0, count, obj, () =>
                {
                    AssertCountry.AssertProperties(obj, new Country
                    {
                        Id = 1,
                        Name = "Austria"
                    });

                    Assert.AreEqual(1, repo.CountSeededObjects());
                    Assert.AreEqual(1, repo.CountSeededObjects(typeof(Country)));
                    Assert.AreEqual(0, repo.CountSeededObjects(typeof(Project)));
                });

                AssertHelper.AssertIf(typeof(Country), 1, count, obj, () =>
                {
                    AssertCountry.AssertProperties(obj, new Country
                    {
                        Id = 2,
                        Name = "Germany"
                    });

                    Assert.AreEqual(2, repo.CountSeededObjects());
                    Assert.AreEqual(2, repo.CountSeededObjects(typeof(Country)));
                    Assert.AreEqual(0, repo.CountSeededObjects(typeof(Project)));
                });

                AssertHelper.AssertIf(typeof(Project), 0, count, obj, () =>
                {
                    AssertProject.AssertProperties(obj, new Project
                    {
                        Id = 1,
                        Name = "Project1",
                        CountryId = 1
                    });

                    Assert.AreEqual(3, repo.CountSeededObjects());
                    Assert.AreEqual(2, repo.CountSeededObjects(typeof(Country)));
                    Assert.AreEqual(1, repo.CountSeededObjects(typeof(Project)));
                });

                AssertHelper.AssertIf(typeof(Project), 1, count, obj, () =>
                {
                    AssertProject.AssertProperties(obj, new Project
                    {
                        Id = 2,
                        Name = "Project2",
                        CountryId = 2
                    });

                    Assert.AreEqual(4, repo.CountSeededObjects());
                    Assert.AreEqual(2, repo.CountSeededObjects(typeof(Country)));
                    Assert.AreEqual(2, repo.CountSeededObjects(typeof(Project)));
                });

            }, (type, repo) =>
            {

            }, null);

            var cherrySeedConfiguration = new CherrySeedConfiguration(cfg =>
            {
                cfg.WithSpecFlowConfiguration();
                cfg.WithRepository(assertRepository);
                cfg.WithCountryAndProjectEntities();

                cfg.ForEntity<Project>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<Country>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();
            });

            var cherrySeeder = cherrySeedConfiguration.CreateSeeder();
            cherrySeeder.Seed("Country", countryTable);
            cherrySeeder.Seed("Project", projectTable);
        }

        [TestMethod]
        public void SeedAndClear_ClearedSuccessfully()
        {
            var countryTable = ObjectMother.CreateCountryTable(table =>
            {
                table.AddRow("C1", "Austria");
                table.AddRow("C2", "Germany");
            });

            var projectTable = ObjectMother.CreateProjectTable(table =>
            {
                table.AddRow("P1", "Project1", "C1");
                table.AddRow("P2", "Project2", "C2");
            });

            var assertRepository = new AssertRepository((obj, count, repo) =>
            {
                AssertHelper.AssertIf(typeof(Project), 1, count, obj, () =>
                {
                    Assert.AreEqual(4, repo.CountSeededObjects());
                });

            }, (type, repo) =>
            {
            }, null);

            var cherrySeedConfiguration = new CherrySeedConfiguration(cfg =>
            {
                cfg.WithSpecFlowConfiguration();
                cfg.WithRepository(assertRepository);
                cfg.WithCountryAndProjectEntities();

                cfg.ForEntity<Project>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<Country>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();
            });

            var cherrySeeder = cherrySeedConfiguration.CreateSeeder();
            cherrySeeder.Seed("Country", countryTable);
            cherrySeeder.Seed("Project", projectTable);
            cherrySeeder.Clear();

            Assert.AreEqual(0, assertRepository.CountSeededObjects());
        }
    }
}
