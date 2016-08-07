using System;

namespace CherrySeed.TypeTransformations
{
    public class BooleanTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return bool.Parse(str);
        }
    }
}