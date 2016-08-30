using System;

namespace CherrySeed.TypeTransformations
{
    class DoubleTransformation : TypeTransformationBase
    {
        public override object Transform(Type type, string str)
        {
            return double.Parse(str);
        }
    }
}