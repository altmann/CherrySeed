using System;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.Asserts
{
    public static class AssertProject
    {
        public static void AssertProperties(object obj, int expectedCustomerId)
        {
            var project = obj as Project;

            Assert.AreEqual(expectedCustomerId, project.CustomerId);
        } 
    }
}