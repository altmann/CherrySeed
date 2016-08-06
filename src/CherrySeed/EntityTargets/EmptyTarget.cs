using System;

namespace CherrySeed.EntityTargets
{
    public class EmptyTarget : ICreateEntityTarget, IRemoveEntitiesTarget
    {
        public void SaveEntity(object obj)
        {
            
        }

        public void RemoveEntities(Type type)
        {
            
        }
    }
}