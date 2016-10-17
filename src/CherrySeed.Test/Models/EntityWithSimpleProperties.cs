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

    public class EntityWithNotSupportedTypeProperty
    {
        public uint UintProperty { get; set; }
    }

    /// <summary>
    /// ///
    /// </summary>

    public class EntityWithConformIntPk
    {
        public int Id { get; set; }
    }

    public class EntityWithConformIntPk2
    {
        public int ID { get; set; }
    }

    public class EntityWithConformIntPk3
    {
        public int EntityWithConformIntPk3Id { get; set; }
    }

    public class EntityWithConformIntPk4
    {
        public int EntityWithConformIntPk4ID { get; set; }
    }

    public class EntityWithConformStringPk
    {
        public string Id { get; set; }
    }

    public class EntityWithConformGuidPk
    {
        public Guid Id { get; set; }
    }

    public class EntityWithUnconformIntPk
    {
        public int CustomId { get; set; }
    }

    public class EntityWithTReference<T>
    {
        public T ReferenceId { get; set; }
    }

    public class EntityWithIntReference : EntityWithTReference<int>
    { }

    public class EntityWithGuidReference : EntityWithTReference<Guid>
    { }

    public class EntityWithStringReference : EntityWithTReference<string>
    { }

    public class EntityWithTReferenceModel<T>
    {
        public T ReferenceModel { get; set; }
    }

    public class EntityWithStringReferenceModel : EntityWithTReferenceModel<EntityWithConformStringPk>
    { }

    public class EntityWithGuidReferenceModel : EntityWithTReferenceModel<EntityWithConformGuidPk>
    { }

    public class EntityWithIntReferenceModel : EntityWithTReferenceModel<EntityWithConformIntPk>
    { }
}