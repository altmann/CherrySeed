using System;

namespace CherrySeed.TypeTransformations
{
    public class DateTimeTransformation : TypeTransformationBase
    {
        public override object Transform(Type type, string str)
        {
            return DateTime.Parse(str);
        }
    }
}