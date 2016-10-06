using System;
using CherrySeed.Repositories;

namespace CherrySeed.Test.Base.Repositories
{
    public class EmptyRepository : IRepository
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