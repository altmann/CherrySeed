using System;
using CherrySeed.TypeTransformations;

namespace CherrySeed.Test.Mocks
{
    public class CustomTypeTransformation<T> : ITypeTransformation
    {
        private readonly T _newValue;

        public CustomTypeTransformation(T newValue)
        {
            _newValue = newValue;
        }

        public object Transform(Type type, string str)
        {
            return _newValue;
        }

        public object TransformNullable(Type type, string str)
        {
            return _newValue;
        }
    }
}