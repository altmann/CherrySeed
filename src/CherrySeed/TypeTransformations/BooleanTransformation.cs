using System;

namespace CherrySeed.TypeTransformations
{
    public class BooleanTransformation : TypeTransformationBase
    {
        public override object Transform(Type type, string str)
        {
            return bool.Parse(str);
        }
    }
}