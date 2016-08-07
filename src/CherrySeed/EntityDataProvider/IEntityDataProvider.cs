using System.Collections.Generic;

namespace CherrySeed.EntityDataProvider
{
    public interface IEntityDataProvider
    {
        List<EntityData> GetEntityData();
    }
}