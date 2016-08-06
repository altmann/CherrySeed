using System;
using System.Linq.Expressions;
using CherrySeed.Utils;

namespace CherrySeed.EntitySettings
{
    public class ReferenceSetting
    {
        public string ReferenceName { get; set; }
        public Type ReferenceType { get; set; }
    }

    public class ReferenceSetting<T> : ReferenceSetting
    {
        public ReferenceSetting(Expression<Func<T, object>> referenceMember, Type referenceType)
        {
            ReferenceName = ReflectionUtil.GetMemberName(referenceMember);
            ReferenceType = referenceType;
        }
    }
}