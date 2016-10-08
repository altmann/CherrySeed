using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntitySettings;
using CherrySeed.IdMappings;
using CherrySeed.TypeTransformations;
using CherrySeed.Utils;

namespace CherrySeed.ObjectTransformation
{
    class ObjectTransformation
    {
        private readonly TypeTransformationProvider _typeTransformationProvider;
        private readonly IdMappingProvider _idMappingProvider;

        public ObjectTransformation(
            TypeTransformationProvider typeTransformationProvider,
            IdMappingProvider idMappingProvider)
        {
            _typeTransformationProvider = typeTransformationProvider;
            _idMappingProvider = idMappingProvider;
        }

        public object Transform(Dictionary<string, string> inputDictionary, Type outputType, EntitySetting entitySetting)
        {
            var outputObject = Activator.CreateInstance(outputType);

            // Set default values
            foreach (var defaultValueSetting in entitySetting.DefaultValueSettings)
            {
                var propertyName = defaultValueSetting.PropertyName;
                var defaultValueProvider = defaultValueSetting.Provider;
                var defaultValue = defaultValueProvider.GetDefaultValue();

                ReflectionUtil.SetProperty(outputObject, propertyName, defaultValue);
            }

            // Set values from data provider
            foreach (var inputKeyValuePair in inputDictionary)
            {
                var propertyName = inputKeyValuePair.Key;
                var propertyValue = inputKeyValuePair.Value;

                SetProperty(outputObject, propertyName, propertyValue, entitySetting);
            }

            return outputObject;
        }

        private void SetProperty(object obj, string propertyName, string propertyValue, EntitySetting entitySetting)
        {
            try
            {
                var propertyType = ReflectionUtil.GetPropertyType(obj.GetType(), propertyName);

                if (IsPrimaryKey(propertyName, entitySetting.PrimaryKey))
                {
                    if (entitySetting.IdGeneration.Generator != null)
                    {
                        ReflectionUtil.SetProperty(obj, propertyName, entitySetting.IdGeneration.Generator.Generate());
                    }
                }
                else if (IsForeignKey(propertyName, entitySetting.References))
                {
                    var referenceSetting = entitySetting.References.First(rd => rd.ReferenceName == propertyName);
                    var foreignKeyId = _idMappingProvider.GetRepositoryId(referenceSetting.ReferenceType, propertyValue);

                    if (ReflectionUtil.IsReferenceType(propertyType))
                    {
                        var referenceModel = entitySetting.Repository.LoadEntity(propertyType, foreignKeyId);
                        ReflectionUtil.SetProperty(obj, propertyName, referenceModel);
                    }
                    else
                    {
                        ReflectionUtil.SetProperty(obj, propertyName, foreignKeyId);
                    }
                }
                else
                {
                    var simpleTransformation = _typeTransformationProvider.GetSimpleTransformation(propertyType);

                    var typedPropertyValue = ReflectionUtil.IsNullableValueType(propertyType)
                        ? simpleTransformation.TransformNullable(propertyType, propertyValue)
                        : simpleTransformation.Transform(propertyType, propertyValue);

                    ReflectionUtil.SetProperty(obj, propertyName, typedPropertyValue);
                }
            }
            catch (Exception ex)
            {
                throw new PropertyTransformationException(obj.GetType(), propertyName, propertyValue, ex);
            }
        }

        private bool IsPrimaryKey(string propertyName, PrimaryKeySetting primaryKeySetting)
        {
            var primaryKeyName = primaryKeySetting.PrimaryKeyName;
            return propertyName == primaryKeyName;
        }

        private bool IsForeignKey(string propertyName, List<ReferenceSetting> referenceDescriptions)
        {
            return referenceDescriptions.Select(rd => rd.ReferenceName).Contains(propertyName);
        }
    }
}
