using System;

namespace CherrySeed.ObjectTransformation
{
    public class PropertyMappingException : Exception
    {
        public PropertyMappingException(Type type, string propertyName, object propertyValue, string reason)
            : base($"Set Property '{propertyName}' of type '{type}' to value '{propertyValue}' failed. Reason: {reason}")
        {
            
        }
    }
}