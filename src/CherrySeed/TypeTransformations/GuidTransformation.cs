using System;

namespace CherrySeed.TypeTransformations
{
    class GuidTransformation : TypeTransformationBase
    {
        public override object Transform(Type type, string str)
        {
            return Guid.Parse(str);
        }
    }
}