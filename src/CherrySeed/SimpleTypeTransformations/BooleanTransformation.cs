using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public class BooleanTransformation : ITypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return bool.Parse(str);
        }
    }
}