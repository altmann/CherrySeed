using System;

namespace CherrySeed.TypeTransformations
{
    abstract class TypeTransformationBase : ITypeTransformation
    {
        public abstract object Transform(Type type, string str);

        public object TransformNullable(Type type, string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            return Transform(type, str);
        }
    }
}