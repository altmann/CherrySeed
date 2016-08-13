using System;

namespace CherrySeed.TypeTransformations
{
    public class DecimalTransformation : TypeTransformationBase
    {
        public override object Transform(Type type, string str)
        {
            return decimal.Parse(str);
        }
    }
}