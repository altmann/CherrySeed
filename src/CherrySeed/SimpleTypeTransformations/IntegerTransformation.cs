using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public class IntegerTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return int.Parse(str);
        }
    }
}