using System;
using System.Collections.Generic;
using CherrySeed.Utils;

namespace CherrySeed.TypeTransformations
{
    class TypeTransformationProvider
    {
        private readonly Dictionary<Type, ITypeTransformation> _simpleTypeTransformations;

        public TypeTransformationProvider(Dictionary<Type, ITypeTransformation> simpleTypeTransformations)
        {
            _simpleTypeTransformations = simpleTypeTransformations;
        }

        public ITypeTransformation GetSimpleTransformation(Type type)
        {
            var relevantType = GetRelevantType(type);
            var lookupType = relevantType.IsEnum
                ? typeof(Enum)
                : relevantType;

            if (!_simpleTypeTransformations.ContainsKey(lookupType))
            {
                throw new NotSupportedException($"Transformation of type '{type}' is currently not supported");
            }

            return _simpleTypeTransformations[lookupType];
        }

        private Type GetRelevantType(Type type)
        {
            if (ReflectionUtil.IsNullableValueType(type))
                return type.GetGenericArguments()[0];

            return type;
        }
    }
}