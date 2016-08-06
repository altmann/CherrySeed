using System;

namespace CherrySeed.EntityTargets
{
    public class EmptyTarget : ICreateEntityTarget, IRemoveEntitiesTarget
    {
        public void Save(object obj)
        {
            
        }

        public void RemoveEntities(Type type)
        {
            
        }
    }
}