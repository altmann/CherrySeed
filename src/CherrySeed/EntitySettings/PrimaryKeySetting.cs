using System;
using System.Linq.Expressions;
using CherrySeed.Utils;

namespace CherrySeed.EntitySettings
{
    public class PrimaryKeySetting
    {
        public string PrimaryKeyName { get; set; }

        public PrimaryKeySetting(string primaryKeyName)
        {
            PrimaryKeyName = primaryKeyName;
        }
    }

    public class PrimaryKeySetting<T> : PrimaryKeySetting
    {
        public PrimaryKeySetting(Expression<Func<T, object>> primaryKeyMember)
            : base(ReflectionUtil.GetMemberName(primaryKeyMember))
        { }
    }
}