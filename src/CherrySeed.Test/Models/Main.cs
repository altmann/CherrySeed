using System;

namespace CherrySeed.Test.Models
{
    public enum MyEnum
    {
        EnumValue1,
        EnumValue2
    }

    public class Main
    {
        public int Id { get; set; }
        public string MyString { get; set; }
        public bool MyBool { get; set; }
        public DateTime MyDateTime { get; set; }
        public int SubId { get; set; }
        public double MyDouble { get; set; }
        public decimal MyDecimal { get; set; }
        public MyEnum MyEnum1 { get; set; }
        public MyEnum MyEnum2 { get; set; }
    }
}