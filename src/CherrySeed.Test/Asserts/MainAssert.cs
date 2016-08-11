using System;
using System.CodeDom;
using CherrySeed.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherrySeed.Test.Asserts
{
    public static class AssertMain
    {
        public static void AssertProperties(object obj, DateTime expectedDateTime, string expectedString, bool expectedBool, decimal expectedDecimal, double expectedDouble, MyEnum expectedEnum1, MyEnum expectedEnum2)
        {
            var main = obj as Main;

            Assert.AreEqual(expectedString, main.MyString);
            Assert.AreEqual(expectedDateTime, main.MyDateTime);
            Assert.AreEqual(expectedBool, main.MyBool);
            Assert.AreEqual(expectedDecimal, main.MyDecimal);
            Assert.AreEqual(expectedDouble, main.MyDouble);
            Assert.AreEqual(expectedEnum1, main.MyEnum1);
            Assert.AreEqual(expectedEnum2, main.MyEnum2);
        } 
    }

    public static class AssertSub
    {
        public static void AssertProperties(object obj, string expectedString)
        {
            var sub = obj as Sub;

            Assert.AreEqual(expectedString, sub.MyString);
        }
    }
}