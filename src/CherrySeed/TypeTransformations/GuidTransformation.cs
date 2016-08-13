using System;

namespace CherrySeed.TypeTransformations
{
    public class GuidTransformation : TypeTransformationBase
    {
        public override object Transform(Type type, string str)
        {
            return Guid.Parse(str);
        }
    }
}