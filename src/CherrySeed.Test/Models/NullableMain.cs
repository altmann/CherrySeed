using System;

namespace CherrySeed.Test.Models
{
    public class NullableMain
    {
        public int Id { get; set; }
        public int? MyInt { get; set; }
        public string MyString { get; set; }
        public string MyString2 { get; set; }
        public bool? MyBool { get; set; }
        public DateTime? MyDateTime { get; set; }
        public int? SubId { get; set; }
        public double? MyDouble { get; set; }
        public decimal? MyDecimal { get; set; }
        public MyEnum? MyEnum1 { get; set; }
        public MyEnum? MyEnum2 { get; set; }
    }
}