using System;

namespace CherrySeed.Repositories
{
    public class EmptyTarget : ICreateRepository, IRemoveRepository
    {
        public void SaveEntity(object obj)
        {
            
        }

        public void RemoveEntities(Type type)
        {
            
        }
    }
}