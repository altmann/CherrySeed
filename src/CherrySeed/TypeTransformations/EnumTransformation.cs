using System;

namespace CherrySeed.TypeTransformations
{
    public class EnumTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return Enum.Parse(type, str);
        }
    }
}