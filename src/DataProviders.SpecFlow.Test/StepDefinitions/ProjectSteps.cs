using TechTalk.SpecFlow;

namespace CherrySeed.DataProviders.SpecFlow.Test.StepDefinitions
{
    [Binding]
    public class ProjectSteps
    {
        private readonly CherrySeedDriver _cherrySeedDriver;
        public ProjectSteps(CherrySeedDriver cherrySeedDriver)
        {
            _cherrySeedDriver = cherrySeedDriver;
        }

        [Given(@"the following entries of Country exist")]
        public void GivenTheFollowingEntriesOfCountryExist(Table table)
        {
            _cherrySeedDriver.Seed("Country", table);
        }
        
        [Given(@"the following entries of Project exist")]
        public void GivenTheFollowingEntriesOfProjectExist(Table table)
        {
            _cherrySeedDriver.Seed("Project", table);
        }

        [BeforeScenario(Order = 2)]
        public void ClearData()
        {
            _cherrySeedDriver.Clear();
        }
    }
}
