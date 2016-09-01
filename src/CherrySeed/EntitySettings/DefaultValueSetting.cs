using System;
using System.Linq.Expressions;
using CherrySeed.DefaultValues;
using CherrySeed.Utils;

namespace CherrySeed.EntitySettings
{
    public class DefaultValueSetting
    {
        public string PropertyName { get; set; }
        public IDefaultValueProvider Provider { get; set; }
    }

    public class DefaultValueSetting<T> : DefaultValueSetting
    {
        public DefaultValueSetting(Expression<Func<T, object>> fieldExpression, IDefaultValueProvider provider)
        {
            PropertyName = ReflectionUtil.GetMemberName(fieldExpression);
            Provider = provider;
        }
    }
}