using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.Infrastructure
{
    [TestClass]
    public class SetupAssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            CultureUtil.SetGermanCulture();
        }
    }
}