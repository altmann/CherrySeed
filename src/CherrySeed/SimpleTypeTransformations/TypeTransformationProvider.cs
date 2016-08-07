using System;
using System.Collections.Generic;

namespace CherrySeed.SimpleTypeTransformations
{
    public class TypeTransformationProvider
    {
        private readonly Dictionary<Type, ITypeTransformation> _simpleTypeTransformations;

        public TypeTransformationProvider(Dictionary<Type, ITypeTransformation> simpleTypeTransformations)
        {
            _simpleTypeTransformations = new Dictionary<Type, ITypeTransformation>
            {
                { typeof(string), new StringTransformation() },
                { typeof(int), new IntegerTransformation() },
                { typeof(DateTime), new DateTimeTransformation() },
                { typeof(bool), new BooleanTransformation() },
                { typeof(Guid), new GuidTransformation() },
                { typeof(Enum), new EnumTransformation() }
            };

            foreach (var simpleTypeTransformation in simpleTypeTransformations)
            {
                var type = simpleTypeTransformation.Key;
                var transformation = simpleTypeTransformation.Value;

                if (_simpleTypeTransformations.ContainsKey(type))
                {
                    _simpleTypeTransformations[type] = transformation;
                }
                else
                {
                    _simpleTypeTransformations.Add(type, transformation);
                }
            }
        }

        public ITypeTransformation GetSimpleTransformation(Type type)
        {
            return _simpleTypeTransformations[type];
        }
    }
}