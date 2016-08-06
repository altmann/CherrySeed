using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public class StringTransformation : ISimpleTypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return str;
        }
    }
}