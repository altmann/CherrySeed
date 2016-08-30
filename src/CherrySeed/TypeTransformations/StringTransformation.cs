using System;

namespace CherrySeed.TypeTransformations
{
    class StringTransformation : ITypeTransformation
    {
        private readonly string _emptyStringMarker;

        public StringTransformation(string emptyStringMarker)
        {
            _emptyStringMarker = emptyStringMarker;
        }

        public object Transform(Type type, string str)
        {
            if(string.IsNullOrEmpty(str))
                return null;

            if (str == _emptyStringMarker)
                return "";

            return str;
        }

        public object TransformNullable(Type type, string str)
        {
            // Not implemented because Nullable of string is not possible
            throw new NotImplementedException();
        }
    }
}