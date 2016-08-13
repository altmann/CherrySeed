using System;

namespace CherrySeed.TypeTransformations
{
    public class IntegerTransformation : TypeTransformationBase
    {
        public override object Transform(Type type, string str)
        {
            return int.Parse(str);
        }
    }
}