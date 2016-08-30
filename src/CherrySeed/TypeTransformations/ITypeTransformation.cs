using System;

namespace CherrySeed.TypeTransformations
{
    public interface ITypeTransformation
    {
        object Transform(Type type, string str);
        object TransformNullable(Type type, string str);
    }
}