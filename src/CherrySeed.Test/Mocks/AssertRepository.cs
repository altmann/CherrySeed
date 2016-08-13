using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.Repositories;
using CherrySeed.Test.Models;

namespace CherrySeed.Test.Mocks
{
    public class AssertRepository : IRepository
    {
        private readonly Action<object, int, Dictionary<Type, List<object>>> _saveEntityFunc;
        private readonly Action<Type> _removeEntitiesFunc;
        private readonly Dictionary<Type, int> _countDictionary;
        private readonly Dictionary<Type, List<object>> _entities; 

        public AssertRepository(Action<object, int, Dictionary<Type, List<object>>> saveEntityFunc, Action<Type> removeEntitiesFunc)
        {
            _saveEntityFunc = saveEntityFunc;
            _removeEntitiesFunc = removeEntitiesFunc;
            _countDictionary = new Dictionary<Type, int>();
            _entities = new Dictionary<Type, List<object>>();
        }

        public void SaveEntity(object obj)
        {
            var type = obj.GetType();

            if (_countDictionary.ContainsKey(type))
            {
                _countDictionary[type]++;
            }
            else
            {
                _countDictionary.Add(type, 0);
            }

            _saveEntityFunc(obj, _countDictionary[type], _entities);

            if(_entities.ContainsKey(type))
                _entities[type].Add(obj);
            else
                _entities.Add(type, new List<object> {obj});
        }

        public void RemoveEntities(Type type)
        {
            _removeEntitiesFunc(type);
            
            if(_entities.ContainsKey(type))
                _entities[type].Clear();
        }

        public object LoadEntity(Type type, object id)
        {
            if(!_entities.ContainsKey(type))
                throw new InvalidOperationException("entity type not found in dict");

            if(type != typeof(Sub))
                throw new InvalidOperationException("this entity is not supported for model reference");

            return _entities[type].First(e => ((Sub) e).Id == (int)id);
        }
    }
}