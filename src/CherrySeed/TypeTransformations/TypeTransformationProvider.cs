using System;
using System.Collections.Generic;
using CherrySeed.Utils;

namespace CherrySeed.TypeTransformations
{
    public class TypeTransformationProvider
    {
        private readonly Dictionary<Type, ITypeTransformation> _simpleTypeTransformations;

        public TypeTransformationProvider(Dictionary<Type, ITypeTransformation> simpleTypeTransformations)
        {
            _simpleTypeTransformations = simpleTypeTransformations;
        }

        public ITypeTransformation GetSimpleTransformation(Type type)
        {
            var relevantType = GetRelevantType(type);

            return relevantType.IsEnum
                ? _simpleTypeTransformations[typeof(Enum)]
                : _simpleTypeTransformations[relevantType];
        }

        private Type GetRelevantType(Type type)
        {
            if (ReflectionUtil.IsNullableValueType(type))
                return type.GetGenericArguments()[0];

            return type;
        }
    }
}