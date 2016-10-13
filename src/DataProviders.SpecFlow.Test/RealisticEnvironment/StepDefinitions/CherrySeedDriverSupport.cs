using BoDi;
using CherrySeed.Configuration;
using CherrySeed.DataProviders.SpecFlow.Test.Entities;
using CherrySeed.Test.Base.Repositories;
using CherrySeed.DataProviders.SpecFlow.Test.Common;
using TechTalk.SpecFlow;

namespace CherrySeed.DataProviders.SpecFlow.Test.StepDefinitions
{
    [Binding]
    public class CherrySeedDriverSupport
    {
        private readonly IObjectContainer _objectContainer;

        public CherrySeedDriverSupport(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario(Order = 1)]
        public void InitializeCherrySeedDriver()
        {
            // First way with default CherrySeed instance and extensions
            var cherrySeedConfig = new CherrySeedConfiguration(cfg =>
            {
                cfg.WithSpecFlowConfiguration();
                cfg.WithRepository(new EmptyRepository());
                cfg.WithCountryAndProjectEntities();
            });
            var seeder = cherrySeedConfig.CreateSeeder();
            _objectContainer.RegisterInstanceAs(seeder);

            // Second way with CherrySeedDriver
            var cherrySeedDriver = new CherrySeedDriver(cfg =>
            {
                cfg.WithRepository(new EmptyRepository());
                cfg.WithCountryAndProjectEntities();
            });

            _objectContainer.RegisterInstanceAs(cherrySeedDriver);
        }
    }
}