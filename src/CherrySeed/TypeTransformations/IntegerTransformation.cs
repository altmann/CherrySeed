using System;

namespace CherrySeed.TypeTransformations
{
    public class IntegerTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return int.Parse(str);
        }
    }
}