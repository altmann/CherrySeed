using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public interface ITypeTransformation
    {
        object Transform(Type type, string str);
    }
}