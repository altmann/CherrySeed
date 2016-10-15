using System;

namespace CherrySeed.Test.Models
{
    public class EntityWithSimpleProperties
    {
        public string MyString { get; set; }
        public bool MyBool { get; set; }
        public DateTime MyDateTime { get; set; }
        public int MyInteger { get; set; }
        public double MyDouble { get; set; }
        public decimal MyDecimal { get; set; }
    }

    public class EntityWithNullableProperties
    {
        public string MyString { get; set; }
        public bool? MyBool { get; set; }
        public DateTime? MyDateTime { get; set; }
        public int? MyInteger { get; set; }
        public double? MyDouble { get; set; }
        public decimal? MyDecimal { get; set; }
    }

    public enum TestEnum
    {
        EnumValue1,
        EnumValue2
    }

    public class EntityWithEnumProperty
    {
        public TestEnum EnumProperty1 { get; set; }
        public TestEnum EnumProperty2 { get; set; }
    }

    public class EntityWithNullableEnumProperty
    {
        public TestEnum? EnumProperty1 { get; set; }
        public TestEnum? EnumProperty2 { get; set; }
    }
}