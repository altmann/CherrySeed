using System;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.Asserts
{
    public static class AssertMainWithModelReference
    {
        public static void AssertProperties(object obj, object expectedSub)
        {
            var main = obj as MainWithModelReference;

            Assert.AreEqual(expectedSub, main.Sub);
        } 
    }
}