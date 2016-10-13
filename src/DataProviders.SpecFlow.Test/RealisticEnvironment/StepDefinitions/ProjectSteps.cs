using TechTalk.SpecFlow;

namespace CherrySeed.DataProviders.SpecFlow.Test.RealisticEnvironment.StepDefinitions
{
    [Binding]
    public class ProjectSteps
    {
        private readonly ICherrySeeder _cherrySeeder;

        public ProjectSteps(ICherrySeeder cherrySeeder)
        {
            _cherrySeeder = cherrySeeder;
        }

        [Given(@"the following countries exist")]
        public void GivenTheFollowingEntriesOfCountryExist(Table table)
        {
            _cherrySeeder.Seed("Country", table);
        }
        
        [Given(@"the following projects exist")]
        public void GivenTheFollowingEntriesOfProjectExist(Table table)
        {
            _cherrySeeder.Seed("Project", table);
        }

        [BeforeScenario(Order = 2)]
        public void ClearData()
        {
            _cherrySeeder.Clear();
        }
    }
}
