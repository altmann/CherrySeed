using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.Asserts
{
    public static class AssertHelper
    {
        public static void AssertIf(Type shouldEntityType, int shouldCount, int actualCount, object obj, Action action)
        {
            if (!(obj.GetType() == shouldEntityType))
            {
                return;
            }

            if (shouldCount != actualCount)
            {
                return;
            }

            action();
        }

        public static void AssertExceptionWithMessage(Exception actualException, Type expectedException,
            string expectedMessage)
        {
            Assert.AreEqual(expectedException, actualException.GetType(), "Asset: Expected exception is not actual exception");
            Assert.IsTrue(actualException.Message.Contains(expectedMessage), "Asset: Expected exception message is not actual exception message");
        }

        public static void AssertException(Exception actualException, Type expectedException)
        {
            Assert.AreEqual(expectedException, actualException.GetType(), "Asset: Expected exception is not actual exception");
        }

        public static void TryCatch(Action tryAction, Action<Exception> catchAction)
        {
            try
            {
                tryAction();
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex)
            {
                catchAction(ex);
            }
        }
    }
}