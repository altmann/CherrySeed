using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.Asserts
{
    public static class EntityAsserts
    {
        public static void AssertEntityWithSimpleProperties(EntityWithSimpleProperties actual, EntityWithSimpleProperties expected)
        {
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.MyString, actual.MyString);
            Assert.AreEqual(expected.MyBool, actual.MyBool);
            Assert.AreEqual(expected.MyDateTime, actual.MyDateTime);
            Assert.AreEqual(expected.MyDecimal, actual.MyDecimal);
            Assert.AreEqual(expected.MyDouble, actual.MyDouble);
            Assert.AreEqual(expected.MyInteger, actual.MyInteger);
        }

        public static void AssertEntityWithNullableProperties(EntityWithNullableProperties actual, EntityWithNullableProperties expected)
        {
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.MyString, actual.MyString);
            Assert.AreEqual(expected.MyBool, actual.MyBool);
            Assert.AreEqual(expected.MyDateTime, actual.MyDateTime);
            Assert.AreEqual(expected.MyDecimal, actual.MyDecimal);
            Assert.AreEqual(expected.MyDouble, actual.MyDouble);
            Assert.AreEqual(expected.MyInteger, actual.MyInteger);
        }

        public static void AssertEntityWithEnumProperty(EntityWithEnumProperty actual, EntityWithEnumProperty expected)
        {
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.EnumProperty1, actual.EnumProperty1);
            Assert.AreEqual(expected.EnumProperty2, actual.EnumProperty2);
        }

        public static void AssertEntityWithNullableEnumProperty(EntityWithNullableEnumProperty actual, EntityWithNullableEnumProperty expected)
        {
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.EnumProperty1, actual.EnumProperty1);
            Assert.AreEqual(expected.EnumProperty2, actual.EnumProperty2);
        }
    }
}