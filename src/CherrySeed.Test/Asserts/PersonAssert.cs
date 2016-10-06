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

    public static class AssertEntityWithStringId
    {
        public static void AssertProperties(object obj, string expectedId, string expectedReferenceId)
        {
            var entity = obj as EntityWithStringId;

            Assert.AreEqual(expectedId, entity.Id);
            Assert.AreEqual(expectedReferenceId, entity.AnotherEntityWithStringIdId);
        }
    }

    public static class AssertAnotherEntityWithStringId
    {
        public static void AssertProperties(object obj, string expectedId)
        {
            var entity = obj as AnotherEntityWithStringId;

            Assert.AreEqual(expectedId, entity.Id);
        }
    }
}