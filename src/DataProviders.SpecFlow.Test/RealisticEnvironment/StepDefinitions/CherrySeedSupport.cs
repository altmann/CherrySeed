using BoDi;
using CherrySeed.Configuration;
using CherrySeed.DataProviders.SpecFlow.Test.Common;
using CherrySeed.Test.Base.Repositories;
using TechTalk.SpecFlow;

namespace CherrySeed.DataProviders.SpecFlow.Test.RealisticEnvironment.StepDefinitions
{
    [Binding]
    public class CherrySeedSupport
    {
        private readonly IObjectContainer _objectContainer;

        public CherrySeedSupport(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario(Order = 1)]
        public void InitializeCherrySeedDriver()
        {
            var cherrySeedConfig = new CherrySeedConfiguration(cfg =>
            {
                cfg.WithSpecFlowConfiguration();
                cfg.WithRepository(new EmptyRepository());
                cfg.WithCountryAndProjectEntities();
            });
            var seeder = cherrySeedConfig.CreateSeeder();
            _objectContainer.RegisterInstanceAs(seeder);
        }
    }
}