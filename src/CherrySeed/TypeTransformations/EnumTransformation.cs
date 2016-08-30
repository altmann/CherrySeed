using System;
using CherrySeed.Utils;

namespace CherrySeed.TypeTransformations
{
    class EnumTransformation : TypeTransformationBase
    {
        public override object Transform(Type type, string str)
        {
            var enumType = ReflectionUtil.IsNullableValueType(type)
                ? Nullable.GetUnderlyingType(type)
                : type;

            return Enum.Parse(enumType, str);
        }
    }
}