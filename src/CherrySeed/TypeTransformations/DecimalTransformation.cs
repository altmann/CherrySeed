using System;

namespace CherrySeed.TypeTransformations
{
    public class DecimalTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return decimal.Parse(str);
        }
    }
}