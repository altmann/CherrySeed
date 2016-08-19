using CherrySeed.EntityDataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataProviders.Csv.Test.Asserts
{
    public static class MyAssert
    {
        public static void AssertSimple(EntityData entityData)
        {
            Assert.AreEqual("Simple", entityData.EntityName);
            Assert.AreEqual(2, entityData.Objects.Count);

            Assert.AreEqual(3, entityData.Objects[0].Count);
            Assert.AreEqual("1", entityData.Objects[0]["Field1"]);
            Assert.AreEqual("2", entityData.Objects[0]["Field2"]);
            Assert.AreEqual("3", entityData.Objects[0]["Field3"]);

            Assert.AreEqual(3, entityData.Objects[1].Count);
            Assert.AreEqual("4", entityData.Objects[1]["Field1"]);
            Assert.AreEqual("5", entityData.Objects[1]["Field2"]);
            Assert.AreEqual("6", entityData.Objects[1]["Field3"]);
        }

        public static void AssertSimpleEmpty(EntityData entityData)
        {
            Assert.AreEqual("SimpleEmpty", entityData.EntityName);
            Assert.AreEqual("", entityData.Objects[0]["Field1"]);
            Assert.AreEqual("2", entityData.Objects[0]["Field2"]);
            Assert.AreEqual("", entityData.Objects[0]["Field3"]);
            Assert.AreEqual("4", entityData.Objects[1]["Field1"]);
            Assert.AreEqual("5", entityData.Objects[1]["Field2"]);
            Assert.AreEqual("6", entityData.Objects[1]["Field3"]);
        }

        public static void AssertComplex(EntityData entityData)
        {
            Assert.AreEqual("Complex", entityData.EntityName);

            Assert.AreEqual(4, entityData.Objects[0].Count);
            Assert.AreEqual("Michael Altmann", entityData.Objects[0]["Name"]);
            Assert.AreEqual("2016-06-06", entityData.Objects[0]["Birthdate"]);
            Assert.AreEqual("2017", entityData.Objects[0]["Year"]);
            Assert.AreEqual("12.12", entityData.Objects[0]["DecimalField"]);

            Assert.AreEqual(4, entityData.Objects[1].Count);
            Assert.AreEqual("Simone Altmann", entityData.Objects[1]["Name"]);
            Assert.AreEqual("2012-02-02", entityData.Objects[1]["Birthdate"]);
            Assert.AreEqual("2011", entityData.Objects[1]["Year"]);
            Assert.AreEqual("33.44", entityData.Objects[1]["DecimalField"]);
        }
    }
}