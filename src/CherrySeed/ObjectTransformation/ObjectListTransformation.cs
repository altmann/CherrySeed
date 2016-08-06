using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntitySettings;

namespace CherrySeed.ObjectTransformation
{
    public class ObjectListTransformation
    {
        private readonly ObjectTransformation _objectTransformation;

        public ObjectListTransformation(ObjectTransformation objectTransformation)
        {
            _objectTransformation = objectTransformation;
        }

        public List<object> Transform(Type type, List<Dictionary<string, string>> inputObjects, EntitySetting entitySetting)
        {
            var inputObjectList = inputObjects
                .Select(inputObject => _objectTransformation.Transform(inputObject, type, entitySetting))
                .ToList();

            return inputObjectList;
        }
    }
}