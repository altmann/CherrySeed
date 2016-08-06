using System;

namespace CherrySeed.SimpleTypeTransformations
{
    public class BooleanTransformation : ISimpleTypeTransformation
    {
        public object Transform(Type type, string str)
        {
            return bool.Parse(str);
        }
    }
}