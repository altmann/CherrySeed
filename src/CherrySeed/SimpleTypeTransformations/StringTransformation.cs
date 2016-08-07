using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public class StringTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return str;
        }
    }
}