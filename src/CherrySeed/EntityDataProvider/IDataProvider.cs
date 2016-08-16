using System.Collections.Generic;

namespace CherrySeed.EntityDataProvider
{
    public interface IDataProvider
    {
        List<EntityData> GetEntityData();
    }
}