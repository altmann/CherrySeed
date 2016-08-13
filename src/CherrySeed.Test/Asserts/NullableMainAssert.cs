using System;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.Asserts
{
    public static class AssertNullableMain
    {
        public static void AssertProperties(object obj, int? expectedInt, DateTime? expectedDateTime, string expectedString, string expectedString2, bool? expectedBool, decimal? expectedDecimal, double? expectedDouble, MyEnum? expectedEnum1, MyEnum? expectedEnum2, int? expectedSubId)
        {
            var nullableMain = obj as NullableMain;

            Assert.AreEqual(expectedInt, nullableMain.MyInt);
            Assert.AreEqual(expectedString, nullableMain.MyString);
            Assert.AreEqual(expectedString2, nullableMain.MyString2);
            Assert.AreEqual(expectedDateTime, nullableMain.MyDateTime);
            Assert.AreEqual(expectedBool, nullableMain.MyBool);
            Assert.AreEqual(expectedDecimal, nullableMain.MyDecimal);
            Assert.AreEqual(expectedDouble, nullableMain.MyDouble);
            Assert.AreEqual(expectedEnum1, nullableMain.MyEnum1);
            Assert.AreEqual(expectedEnum2, nullableMain.MyEnum2);
            Assert.AreEqual(expectedSubId, nullableMain.SubId);
        }
    }
}