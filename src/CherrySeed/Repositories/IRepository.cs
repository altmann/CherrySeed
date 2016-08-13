using System;

namespace CherrySeed.Repositories
{
    public interface IRepository
    {
        void SaveEntity(object obj);
        void RemoveEntities(Type type);
        object LoadEntity(Type type, object id);
    }
}