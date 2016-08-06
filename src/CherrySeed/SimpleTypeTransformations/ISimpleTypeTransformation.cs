using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public interface ISimpleTypeTransformation
    {
        object Transform(Type type, string str);
    }
}