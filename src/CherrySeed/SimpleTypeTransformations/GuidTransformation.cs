using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public class GuidTransformation : ISimpleTypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return Guid.Parse(str);
        }
    }
}