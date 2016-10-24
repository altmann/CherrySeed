using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CherrySeed.Repositories;

namespace CherrySeed.Test.Base.Repositories
{
    public class EntityInfo
    {
        public DateTime SeedingDateTime { get; set; }
        public DateTime ClearingDateTime { get; set; }
        public List<object> Entities { get; set; }

        public EntityInfo()
        {
            Entities = new List<object>();
            SeedingDateTime = DateTime.Now;
        }
    }

    public class InMemoryRepository : IRepository
    {
        private readonly Dictionary<Type, EntityInfo> _entities;
        private readonly Func<object, object, bool> _loadEntityFunc;

        public InMemoryRepository()
        {
            _entities = new Dictionary<Type, EntityInfo>();
        }

        public InMemoryRepository(Func<object, object, bool> loadEntityFunc)
            : this()
        {
            _loadEntityFunc = loadEntityFunc;
        }

        public List<object> GetEntities()
        {
            return _entities.Keys.SelectMany(GetEntities).ToList();
        }

        public List<T> GetEntities<T>()
        {
            var entityType = typeof (T);
            return GetEntities(entityType).OfType<T>().ToList();
        }

        private List<object> GetEntities(Type entityType)
        {
            return _entities[entityType].Entities.ToList();
        }

        public DateTime GetSeedingDateTime<T>()
        {
            var entityType = typeof (T);
            return _entities[entityType].SeedingDateTime;
        }

        public DateTime GetClearingDateTime<T>()
        {
            var entityType = typeof(T);
            return _entities[entityType].ClearingDateTime;
        }

        public void SaveEntity(object obj)
        {
            var type = obj.GetType();

            if (_entities.ContainsKey(type))
                _entities[type].Entities.Add(obj);
            else
            {
                _entities.Add(type, new EntityInfo
                {
                    Entities = new List<object> {obj}
                });
            }
        }

        public void RemoveEntities(Type type)
        {
            if (_entities.ContainsKey(type))
            {
                _entities[type].ClearingDateTime = DateTime.Now;
                _entities[type].Entities.Clear();
                Thread.Sleep(100);
            }
        }

        public object LoadEntity(Type type, object id)
        {
            return _entities[type].Entities.First(o => _loadEntityFunc(o, id));
        }

        public int CountSeededObjects<T>()
        {
            var entityType = typeof (T);

            if (_entities.ContainsKey(entityType))
                return _entities[entityType].Entities.Count;

            return 0;
        }

        public int CountSeededObjects()
        {
            return _entities.Values.Select(e => e.Entities.Count).Sum();
        }
    }
}