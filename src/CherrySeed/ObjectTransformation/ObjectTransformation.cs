using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CherrySeed.EntitySettings;
using CherrySeed.IdMappings;
using CherrySeed.TypeTransformations;
using CherrySeed.Utils;

namespace CherrySeed.ObjectTransformation
{
    public class ObjectTransformation
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
                var foreignKeyType = entitySetting.References.First(rd => rd.ReferenceName == propertyName).ReferenceType;
                var foreignKeyId = _idMappingProvider.GetRepositoryId(foreignKeyType, propertyValue);
                ReflectionUtil.SetProperty(obj, propertyName, foreignKeyId);
            }
            else
            {
                var simpleTransformation = propertyType.IsEnum
                    ? _typeTransformationProvider.GetSimpleTransformation(typeof(Enum))
                    : _typeTransformationProvider.GetSimpleTransformation(propertyType);

                var typedPropertyValue = simpleTransformation.Transform(propertyType, propertyValue);
                ReflectionUtil.SetProperty(obj, propertyName, typedPropertyValue);
            }
        }

        private bool IsPrimaryKey(string propertyName, PrimaryKeySetting primaryKeySetting)
        {
            var primaryKeyName = primaryKeySetting.FinalPrimaryKeyName;
            return propertyName == primaryKeyName;
        }

        private bool IsForeignKey(string propertyName, List<ReferenceSetting> referenceDescriptions)
        {
            return referenceDescriptions.Select(rd => rd.ReferenceName).Contains(propertyName);
        }
    }
}
