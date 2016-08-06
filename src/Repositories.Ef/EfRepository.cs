using System;
using System.Data.Entity;
using CherrySeed.EntityTargets;

namespace CherrySeed.Repositories.Ef
{
    public class EfRepository : ICreateEntityTarget, IRemoveEntitiesTarget
    {
        private readonly Func<DbContext> _createDbContextFunc;

        public EfRepository(Func<DbContext> createDbContextFunc)
        {
            _createDbContextFunc = createDbContextFunc;
        }

        public void SaveEntity(object obj)
        {
            using (var dbContext = _createDbContextFunc())
            {
                dbContext.Entry(obj).State = EntityState.Added;
                dbContext.SaveChanges();
            }
        }

        public void RemoveEntities(Type type)
        {
            using (var dbContext = _createDbContextFunc())
            {
                dbContext.Set(type).Load();

                foreach (var obj in dbContext.Set(type))
                {
                    dbContext.Entry(obj).State = EntityState.Deleted;
                }

                dbContext.SaveChanges();
            }
        }
    }
}
