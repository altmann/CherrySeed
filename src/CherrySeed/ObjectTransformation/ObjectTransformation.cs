using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CherrySeed.EntitySettings;
using CherrySeed.IdMappings;
using CherrySeed.TypeTransformations;

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

            var primaryKeyName = entitySetting.PrimaryKey.FinalPrimaryKeyName;
            var referenceDescriptions = entitySetting.References; 

            foreach (var inputKeyValuePair in inputDictionary)
            {
                var propertyName = inputKeyValuePair.Key;
                var propertyValue = inputKeyValuePair.Value;

                if (propertyName != primaryKeyName)
                {
                    //property is not a primary key
                    SetProperty(outputObject, propertyName, propertyValue, referenceDescriptions);
                }
            }

            return outputObject;
        }

        private void SetProperty(object obj, string propertyName, object propertyValue, List<ReferenceSetting> referenceDescriptions)
        {
            var type = obj.GetType();

            try
            {
                var prop = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                if (prop != null && prop.CanWrite)
                {
                    var propertyType = prop.PropertyType;

                    object targetPropertyValue = null;
                    if (IsPropertyForeignKey(propertyName, referenceDescriptions))
                    {
                        //property is a foreign key
                        var foreignKeyType = referenceDescriptions.First(rd => rd.ReferenceName == propertyName).ReferenceType;
                        targetPropertyValue = _idMappingProvider.GetRepositoryId(foreignKeyType, (string)propertyValue);
                    }
                    else
                    {
                        var simpleTransformation = propertyType.IsEnum
                            ? _typeTransformationProvider.GetSimpleTransformation(typeof(Enum))
                            : _typeTransformationProvider.GetSimpleTransformation(propertyType);

                        targetPropertyValue = simpleTransformation.Transform(propertyType, (string)propertyValue);
                    }

                    prop.SetValue(obj, targetPropertyValue, null);
                }
            }
            catch (Exception ex)
            {
                throw new SetPropertyException(type, propertyName, propertyValue, ex);
            }
        }

        private static bool IsPropertyForeignKey(string propertyName, List<ReferenceSetting> referenceDescriptions)
        {
            return referenceDescriptions.Select(rd => rd.ReferenceName).Contains(propertyName);
        }
    }
}
