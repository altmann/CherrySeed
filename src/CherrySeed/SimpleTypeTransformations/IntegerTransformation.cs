using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public class IntegerTransformation : ISimpleTypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return int.Parse(str);
        }
    }
}