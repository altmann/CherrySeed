using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public class DateTimeTransformation : ISimpleTypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return DateTime.Parse(str);
        }
    }
}