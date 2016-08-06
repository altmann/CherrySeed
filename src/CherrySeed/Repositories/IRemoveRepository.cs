using System;

namespace CherrySeed.Repositories
{
    public interface IRemoveRepository
    {
        void RemoveEntities(Type type);
    }
}