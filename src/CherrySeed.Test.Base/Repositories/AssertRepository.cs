using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.Repositories;

namespace CherrySeed.Test.Base.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private Dictionary<Type, List<object>> Entities;
        private readonly Func<Dictionary<Type, List<object>>, Type, object, object> _loadEntityFunc;

        public InMemoryRepository()
        {
            Entities = new Dictionary<Type, List<object>>();
        }

        public InMemoryRepository(Func<Dictionary<Type, List<object>>, Type, object, object> loadEntityFunc)
            : this()
        {
            _loadEntityFunc = loadEntityFunc;
        }

        public List<object> GetEntities()
        {
            return Entities.Keys.SelectMany(GetEntities).ToList();
        }

        public List<T> GetEntities<T>()
        {
            var entityType = typeof (T);
            return GetEntities(entityType).OfType<T>().ToList();
        }

        private List<object> GetEntities(Type entityType)
        {
            return Entities[entityType].ToList();
        }

        public void SaveEntity(object obj)
        {
            var type = obj.GetType();

            if (Entities.ContainsKey(type))
                Entities[type].Add(obj);
            else
                Entities.Add(type, new List<object> { obj });
        }

        public void RemoveEntities(Type type)
        {
            if (Entities.ContainsKey(type))
                Entities[type].Clear();
        }

        public object LoadEntity(Type type, object id)
        {
            return _loadEntityFunc(Entities, type, id);
        }

        public int CountSeededObjects<T>()
        {
            var entityType = typeof (T);

            if (Entities.ContainsKey(entityType))
                return Entities[entityType].Count;

            return 0;
        }

        public int CountSeededObjects()
        {
            return Entities.Values.Select(e => e.Count).Sum();
        }
    }

    public class AssertRepository : IRepository
    {
        private readonly Action<object, int, AssertRepository> _saveEntityAssertFunc;
        private readonly Action<Type, AssertRepository> _removeEntitiesAssertFunc;
        private readonly Func<Dictionary<Type, List<object>>, Type, object, object> _loadEntityFunc;
        private readonly Dictionary<Type, int> _countDictionary;
        public Dictionary<Type, List<object>> Entities { get; }

        public AssertRepository(Action<object, int, AssertRepository> saveEntityAssertFunc, 
            Action<Type, AssertRepository> removeEntitiesAssertFunc,
            Func<Dictionary<Type, List<object>>, Type, object, object> loadEntityFunc)
        {
            _saveEntityAssertFunc = saveEntityAssertFunc;
            _removeEntitiesAssertFunc = removeEntitiesAssertFunc;
            _loadEntityFunc = loadEntityFunc;
            _countDictionary = new Dictionary<Type, int>();
            Entities = new Dictionary<Type, List<object>>();
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

            if (Entities.ContainsKey(type))
                Entities[type].Add(obj);
            else
                Entities.Add(type, new List<object> { obj });

            _saveEntityAssertFunc(obj, _countDictionary[type], this);
        }

        public void RemoveEntities(Type type)
        {
            if(Entities.ContainsKey(type))
                Entities[type].Clear();

            _removeEntitiesAssertFunc(type, this);
        }

        public object LoadEntity(Type type, object id)
        {
            return _loadEntityFunc(Entities, type, id);
        }

        public int CountSeededObjects(Type entityType)
        {
            if(Entities.ContainsKey(entityType))
                return Entities[entityType].Count;

            return 0;
        }

        public int CountSeededObjects()
        {
            return Entities.Values.Select(e => e.Count).Sum();
        }
    }
}