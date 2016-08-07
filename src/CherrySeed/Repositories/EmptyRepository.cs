using System;

namespace CherrySeed.Repositories
{
    public class EmptyRepository : IRepository
    {
        public void SaveEntity(object obj)
        {
            
        }

        public void RemoveEntities(Type type)
        {
            
        }
    }
}