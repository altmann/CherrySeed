using System;

namespace CherrySeed.Repositories
{
    class EmptyRepository : IRepository
    {
        public void SaveEntity(object obj)
        {
            
        }

        public void RemoveEntities(Type type)
        {
            
        }

        public object LoadEntity(Type type, object id)
        {
            return null;
        }
    }
}