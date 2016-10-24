using System;
using System.Collections.Generic;
using CherrySeed.EntitySettings;
using CherrySeed.ObjectTransformation.PropertyHandlers;
using CherrySeed.Utils;

namespace CherrySeed.ObjectTransformation
{
    class ObjectTransformation
    {
        private readonly PropertyHandler _propertyHandler;

        public ObjectTransformation(PropertyHandler propertyHandler)
        {
            _propertyHandler = propertyHandler;
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

                _propertyHandler.SetProperty(outputObject, propertyName, propertyValue, entitySetting);
            }

            return outputObject;
        }
    }
}
