using BoDi;
using CherrySeed.DataProviders.SpecFlow.Test.Entities;
using CherrySeed.Test.Base.Repositories;
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
            var cherrySeedDriver = new CherrySeedDriver(cfg =>
            {
                cfg.WithRepository(new EmptyRepository());

                cfg.ForEntity<Country>()
                    .WithPrimaryKey(e => e.Id);

                cfg.ForEntity<Project>()
                    .WithPrimaryKey(e => e.Id)
                    .WithReference(e => e.CountryId, typeof (Country));
            });

            _objectContainer.RegisterInstanceAs(cherrySeedDriver);
        }
    }
}