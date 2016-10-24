using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntitySettings;
using CherrySeed.IdMappings;
using CherrySeed.TypeTransformations;

namespace CherrySeed.ObjectTransformation.PropertyHandlers
{
    class PropertyHandler
    {
        private readonly Dictionary<Func<string, EntitySetting, bool>, IPropertyHandler> _strategyDictionary;

        public PropertyHandler(IdMappingProvider idMappingProvider, TypeTransformationProvider typeTransformationProvider)
        {
            _strategyDictionary = new Dictionary<Func<string, EntitySetting, bool>, IPropertyHandler>
            {
                { PrimaryKeyHandler.CanHandle, new PrimaryKeyHandler() },
                { ForeignKeyHandler.CanHandle, new ForeignKeyHandler(idMappingProvider) },
                { CopyPropertyValueHandler.CanHandle, new CopyPropertyValueHandler(typeTransformationProvider) }
            };
        }

        public void SetProperty(object obj, string propertyName, string propertyValue, EntitySetting entitySetting)
        {
            foreach (var propertyHandlerPair in _strategyDictionary)
            {
                var x = propertyHandlerPair.Key(propertyName, entitySetting);

                if (x)
                {
                    propertyHandlerPair.Value.Handle(obj, propertyName, propertyValue, entitySetting);
                    return;
                }
            }

            //var strategyKeyValuePair = _strategyDictionary
            //    .FirstOrDefault(s => s.Key(propertyName, entitySetting));

            //if (strategyKeyValuePair != null)
            //{
            //    strategyKeyValuePair.Value.Handle(obj, propertyName, propertyValue, entitySetting);
            //}
        }
    }
}