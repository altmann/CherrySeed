using System;

namespace CherrySeed.TypeTransformations
{
    public class StringTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return str;
        }
    }
}