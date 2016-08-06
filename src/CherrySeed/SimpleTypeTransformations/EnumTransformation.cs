using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public class EnumTransformation : ISimpleTypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return Enum.Parse(type, str);
        }
    }
}