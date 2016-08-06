using System;
using System.Linq.Expressions;
using CherrySeed.Utils;

namespace CherrySeed.EntitySettings
{
    public class ReferenceSetting
    {
        public ReferenceSetting(Expression<Func<object, object>> referenceMember, Type referenceType)
        {
            ReferenceName = ReflectionUtil.GetMemberName(referenceMember);
            ReferenceType = referenceType;
        }

        public string ReferenceName { get; set; }
        public Type ReferenceType { get; set; }
    }
}