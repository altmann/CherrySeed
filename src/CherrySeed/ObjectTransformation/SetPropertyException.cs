using System;

namespace CherrySeed.ObjectTransformation
{
    public class SetPropertyException : Exception
    {
        public SetPropertyException(Type type, string propertyName, object propertyValue, Exception innerException)
            : base($"Setting Property '{propertyName}' of type '{type}' to value '{propertyValue}' failed", innerException)
        {
            
        }
    }
}