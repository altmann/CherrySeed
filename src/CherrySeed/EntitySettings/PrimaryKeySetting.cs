using System;
using System.Linq.Expressions;
using CherrySeed.Utils;

namespace CherrySeed.EntitySettings
{
    public class PrimaryKeySetting
    {
        public PrimaryKeySetting(Expression<Func<object, object>> primaryKeyMember)
        {
            PrimaryKeyName = ReflectionUtil.GetMemberName(primaryKeyMember);
        }

        public PrimaryKeySetting(string primaryKeyName)
        {
            PrimaryKeyName = primaryKeyName;
        }

        public string PrimaryKeyName { get; set; }
    }
}