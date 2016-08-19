using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.Infrastructure
{
    public class CultureUtil
    {
        public static void SetGermanCulture()
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("de-DE");
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
        }
    }

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