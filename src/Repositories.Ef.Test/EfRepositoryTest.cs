using System.Data.Entity;
using System.Linq;
using CherrySeed.Repositories.Ef;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Repositories.Ef.Test
{
    public class Simple
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext()
            : base("name=TestDbContext")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<TestDbContext>());
        }

        public DbSet<Simple> Simple { get; set; }
    }

    [TestClass]
    public class EfRepositoryTest
    {
        [TestMethod]
        public void ClearEntries()
        {
            using (var dbContext = new TestDbContext())
            {
                dbContext.Simple.Add(new Simple() { Name = "Michael"});
                dbContext.SaveChanges();

                var count = dbContext.Simple.Count();
                Assert.IsTrue(count > 0);
            }

            var repository = new EfRepository(() => new TestDbContext());

            repository.RemoveEntities(typeof(Simple));

            using (var dbContext = new TestDbContext())
            {
                var count = dbContext.Simple.Count();
                Assert.AreEqual(0, count);
            }
        }

        [TestMethod]
        public void CreateEntry()
        {
            using (var dbContext = new TestDbContext())
            {
                dbContext.Simple.RemoveRange(dbContext.Simple.ToList());
                dbContext.SaveChanges();

                var count = dbContext.Simple.Count();
                Assert.AreEqual(0, count);
            }

            var repository = new EfRepository(() => new TestDbContext());

            repository.SaveEntity(new Simple { Name = "Michael"});

            using (var dbContext = new TestDbContext())
            {
                var count = dbContext.Simple.Count();
                Assert.AreEqual(1, count);
            }
        }

        [TestMethod]
        public void LoadEntry()
        {
            Simple simple;
            using (var dbContext = new TestDbContext())
            {
                dbContext.Simple.RemoveRange(dbContext.Simple.ToList());
                dbContext.SaveChanges();

                var count = dbContext.Simple.Count();
                Assert.AreEqual(0, count);

                simple = new Simple {Name = "Michael"};
                dbContext.Simple.Add(simple);
                dbContext.SaveChanges();
            }

            var repository = new EfRepository(() => new TestDbContext());

            var result = repository.LoadEntity(typeof(Simple), simple.Id);

            var simpleResult = (Simple) result;
            
            Assert.AreEqual("Michael", simpleResult.Name);
        }
    }
}
