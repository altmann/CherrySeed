using System;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.Asserts
{
    public static class AssertPerson
    {
        public static void AssertProperties(object obj, Guid expectedAddressId)
        {
            var person = obj as Person;

            Assert.AreEqual(expectedAddressId, person.AddressId);
        } 
    }
}