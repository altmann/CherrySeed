using System;
using System.Collections.Generic;
using CherrySeed.Repositories;

namespace CherrySeed.Test.Mocks
{
    public class AssertRepository : IRepository
    {
        private readonly Action<object, int> _saveEntityFunc;
        private readonly Action<Type> _removeEntitiesFunc;
        private readonly Dictionary<Type, int> _countDictionary;

        public AssertRepository(Action<object, int> saveEntityFunc, Action<Type> removeEntitiesFunc)
        {
            _saveEntityFunc = saveEntityFunc;
            _removeEntitiesFunc = removeEntitiesFunc;
            _countDictionary = new Dictionary<Type, int>();
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

            _saveEntityFunc(obj, _countDictionary[type]);
        }

        public void RemoveEntities(Type type)
        {
            _removeEntitiesFunc(type);
        }
    }
}