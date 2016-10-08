using System;

namespace CherrySeed.ObjectTransformation
{
    public class PropertyTransformationException : Exception
    {
        //public PropertyTransformationException(Type type, string propertyName, object propertyValue, string reason, Exception innerException)
        //    : base($"Transformation of Property '{propertyName}' of type '{type}' to value '{propertyValue}' failed. Reason: {reason}", innerException)
        //{ }

        //public PropertyTransformationException(Type type, string propertyName, string reason, Exception innerException)
        //   : base($"Transformation of Property '{propertyName}' of type '{type}' failed. Reason: {reason}", innerException)
        //{ }

        public PropertyTransformationException(Type type, string propertyName, object propertyValue, Exception innerException)
           : base($"Transformation of Property '{propertyName}' of type '{type}' to value '{propertyValue}' failed", innerException)
        { }
    }
}