using System;

namespace CherrySeed.ObjectTransformation
{
    public class PropertyTransformationException : Exception
    {
        public PropertyTransformationException(Type type, string propertyName, object propertyValue, Exception innerException)
           : base($"Transformation of Property '{propertyName}' of type '{type}' to value '{propertyValue}' failed", innerException)
        { }
    }

    public class PrimaryKeyException : Exception
    {
        public PrimaryKeyException(Type type, string propertyName, object propertyValue, Exception innerException)
            : base($"Setting primary key (property name = '{propertyName}') of type '{type}' to value '{propertyValue}' failed", innerException)
        { }
    }

    public class ForeignKeyException : Exception
    {
        public ForeignKeyException(Type type, string propertyName, object propertyValue, Exception innerException)
            : base($"Setting foreign key (property name = '{propertyName}') of type '{type}' to value '{propertyValue}' failed", innerException)
        { }
    }
}