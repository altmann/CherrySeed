using System;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.Asserts
{
    public static class EntityAsserts
    {
        public static void AssertEntityWithSimpleProperties(EntityWithSimpleProperties actual, EntityWithSimpleProperties expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.MyString, actual.MyString);
            Assert.AreEqual(expected.MyBool, actual.MyBool);
            Assert.AreEqual(expected.MyDateTime, actual.MyDateTime);
            Assert.AreEqual(expected.MyDecimal, actual.MyDecimal);
            Assert.AreEqual(expected.MyDouble, actual.MyDouble);
            Assert.AreEqual(expected.MyInteger, actual.MyInteger);
        }

        public static void AssertEntityWithNullableProperties(EntityWithNullableProperties actual, EntityWithNullableProperties expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.MyString, actual.MyString);
            Assert.AreEqual(expected.MyBool, actual.MyBool);
            Assert.AreEqual(expected.MyDateTime, actual.MyDateTime);
            Assert.AreEqual(expected.MyDecimal, actual.MyDecimal);
            Assert.AreEqual(expected.MyDouble, actual.MyDouble);
            Assert.AreEqual(expected.MyInteger, actual.MyInteger);
        }

        public static void AssertEntityWithEnumProperty(EntityWithEnumProperty actual, EntityWithEnumProperty expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.EnumProperty1, actual.EnumProperty1);
            Assert.AreEqual(expected.EnumProperty2, actual.EnumProperty2);
        }

        public static void AssertEntityWithNullableEnumProperty(EntityWithNullableEnumProperty actual, EntityWithNullableEnumProperty expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.EnumProperty1, actual.EnumProperty1);
            Assert.AreEqual(expected.EnumProperty2, actual.EnumProperty2);
        }

        public static void AssertEntityWithNotSupportedProperty(EntityWithNotSupportedTypeProperty actual, EntityWithNotSupportedTypeProperty expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.UintProperty, actual.UintProperty);
        }

        public static void AssertEntityWithConformIntPk(EntityWithConformIntPk actual, EntityWithConformIntPk expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        public static void AssertEntityWithIntReference(EntityWithIntReference actual, EntityWithIntReference expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ReferenceId, actual.ReferenceId);
        }

        public static void AssertEntityWithConformIntPk2(EntityWithConformIntPk2 actual, EntityWithConformIntPk2 expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ID, actual.ID);
        }

        public static void AssertEntityWithConformIntPk3(EntityWithConformIntPk3 actual, EntityWithConformIntPk3 expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.EntityWithConformIntPk3Id, actual.EntityWithConformIntPk3Id);
        }

        public static void AssertEntityWithUnconformIntPk(EntityWithUnconformIntPk actual, EntityWithUnconformIntPk expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.CustomId, actual.CustomId);
        }

        public static void AssertEntityWithConformGuidPk(EntityWithConformGuidPk actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreNotEqual(Guid.Empty, actual.Id);
        }

        public static void AssertEntityWithConformStringPk(EntityWithConformStringPk actual, EntityWithConformStringPk expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
        }
    }
}