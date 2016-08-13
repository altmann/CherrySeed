using System;
using System.Linq.Expressions;
using CherrySeed.Utils;

namespace CherrySeed.EntitySettings
{
    public class ReferenceSetting
    {
        public string ReferenceName { get; set; }
        public Type ReferenceType { get; set; }
        public bool IsModelReference { get; set; }
    }

    public class ReferenceSetting<T> : ReferenceSetting
    {
        public ReferenceSetting(Expression<Func<T, object>> referenceMember, 
            Type referenceType,
            bool isModelReference)
        {
            IsModelReference = isModelReference;
            ReferenceName = ReflectionUtil.GetMemberName(referenceMember);
            ReferenceType = referenceType;
        }
    }
}