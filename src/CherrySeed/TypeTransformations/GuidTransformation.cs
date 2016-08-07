using System;

namespace CherrySeed.TypeTransformations
{
    public class GuidTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return Guid.Parse(str);
        }
    }
}