using System;

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
    }
}