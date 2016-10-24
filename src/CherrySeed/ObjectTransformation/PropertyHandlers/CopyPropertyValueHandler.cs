using System;
using CherrySeed.EntitySettings;
using CherrySeed.TypeTransformations;
using CherrySeed.Utils;

namespace CherrySeed.ObjectTransformation.PropertyHandlers
{
    class CopyPropertyValueHandler : IPropertyHandler
    {
        private readonly TypeTransformationProvider _typeTransformationProvider;

        public CopyPropertyValueHandler(TypeTransformationProvider typeTransformationProvider)
        {
            _typeTransformationProvider = typeTransformationProvider;
        }

        public static bool CanHandle(string propertyName, EntitySetting entitySetting)
        {
            var isPrimaryKey = propertyName == entitySetting.PrimaryKey.PrimaryKeyName;

            if (isPrimaryKey && entitySetting.IdGeneration.IsGeneratorEnabled)
                return false;

            if (isPrimaryKey && entitySetting.IdGeneration.IsDatabaseGenerationEnabled)
                return false;
                
            return true;
        }

        public void Handle(object obj, string propertyName, string propertyValue, EntitySetting entitySetting)
        {
            try
            {
                var propertyType = ReflectionUtil.GetPropertyType(obj.GetType(), propertyName);
                var simpleTransformation = _typeTransformationProvider.GetSimpleTransformation(propertyType);

                var typedPropertyValue = ReflectionUtil.IsNullableValueType(propertyType)
                    ? simpleTransformation.TransformNullable(propertyType, propertyValue)
                    : simpleTransformation.Transform(propertyType, propertyValue);

                ReflectionUtil.SetProperty(obj, propertyName, typedPropertyValue);
            }
            catch (Exception ex)
            {
                throw new PropertyTransformationException(obj.GetType(), propertyName, propertyValue, ex);
            }
        }
    }
}