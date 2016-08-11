using System;

namespace CherrySeed.TypeTransformations
{
    public class DoubleTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return double.Parse(str);
        }
    }
}