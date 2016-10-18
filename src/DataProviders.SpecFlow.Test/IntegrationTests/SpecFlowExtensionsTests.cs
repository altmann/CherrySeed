using CherrySeed.Configuration;
using CherrySeed.DataProviders.SpecFlow.Test.Common;
using CherrySeed.DataProviders.SpecFlow.Test.Entities;
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
            // Arrange
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

            // Act - seeding countries
            var repository = new InMemoryRepository();
            var cherrySeedConfiguration = new CherrySeedConfiguration(cfg =>
            {
                cfg.WithSpecFlowConfiguration();
                cfg.WithRepository(repository);
                cfg.WithCountryAndProjectEntities();

                cfg.ForEntity<Country>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<Project>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();
            });

            var cherrySeeder = cherrySeedConfiguration.CreateSeeder();
            cherrySeeder.Seed("Country", countryTable);

            // Assert - countries
            Assert.AreEqual(2, repository.CountSeededObjects());
            Assert.AreEqual(2, repository.CountSeededObjects<Country>());
            EntityAsserts.AssertCountry(repository.GetEntities<Country>()[0], new Country
            {
                Id = 1,
                Name = "Austria"
            });
            EntityAsserts.AssertCountry(repository.GetEntities<Country>()[1], new Country
            {
                Id = 2,
                Name = "Germany"
            });

            // Act - seeding projects
            cherrySeeder.Seed("Project", projectTable);

            // Assert - projects
            Assert.AreEqual(4, repository.CountSeededObjects());
            Assert.AreEqual(2, repository.CountSeededObjects<Project>());
            EntityAsserts.AssertProject(repository.GetEntities<Project>()[0], new Project
            {
                Id = 1,
                Name = "Project1",
                CountryId = 1
            });
            EntityAsserts.AssertProject(repository.GetEntities<Project>()[1], new Project
            {
                Id = 2,
                Name = "Project2",
                CountryId = 2
            });
        }

        [TestMethod]
        public void SeedAndClear_ClearedSuccessfully()
        {
            // Arrange
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

            var repository = new InMemoryRepository();
            var cherrySeedConfiguration = new CherrySeedConfiguration(cfg =>
            {
                cfg.WithSpecFlowConfiguration();
                cfg.WithRepository(repository);
                cfg.WithCountryAndProjectEntities();

                cfg.ForEntity<Country>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();

                cfg.ForEntity<Project>()
                    .WithPrimaryKeyIdGenerationInApplicationAsInteger();
            });

            var cherrySeeder = cherrySeedConfiguration.CreateSeeder();
            cherrySeeder.Seed("Country", countryTable);
            cherrySeeder.Seed("Project", projectTable);

            Assert.AreEqual(4, repository.CountSeededObjects());

            // Act - seeding countries
            cherrySeeder.Clear();
            Assert.AreEqual(0, repository.CountSeededObjects());
        }
    }
}
