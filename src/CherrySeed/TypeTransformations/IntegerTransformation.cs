using System;

namespace CherrySeed.TypeTransformations
{
    class IntegerTransformation : TypeTransformationBase
    {
        public override object Transform(Type type, string str)
        {
            return int.Parse(str);
        }
    }
}