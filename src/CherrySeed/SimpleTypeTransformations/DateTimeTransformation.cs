using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public class DateTimeTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return DateTime.Parse(str);
        }
    }
}