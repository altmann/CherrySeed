using TechTalk.SpecFlow;

namespace CherrySeed.DataProviders.SpecFlow.Test.StepDefinitions
{
    [Binding]
    public class ProjectSteps
    {
        private readonly CherrySeedDriver _cherrySeedDriver;
        private readonly ICherrySeeder _cherrySeeder;

        public ProjectSteps(CherrySeedDriver cherrySeedDriver, ICherrySeeder cherrySeeder)
        {
            _cherrySeedDriver = cherrySeedDriver;
            _cherrySeeder = cherrySeeder;
        }

        [Given(@"the following entries of Country exist")]
        public void GivenTheFollowingEntriesOfCountryExist(Table table)
        {
            // First way
            _cherrySeeder.Seed("Country", table);

            // Second way
            //_cherrySeedDriver.Seed("Country", table);
        }
        
        [Given(@"the following entries of Project exist")]
        public void GivenTheFollowingEntriesOfProjectExist(Table table)
        {
            // First way
            _cherrySeeder.Seed("Project", table);

            // Second way
            //_cherrySeedDriver.Seed("Project", table);
        }

        [BeforeScenario(Order = 2)]
        public void ClearData()
        {
            // First way
            _cherrySeeder.Clear();

            // Second way
            //_cherrySeedDriver.Clear();
        }
    }
}
