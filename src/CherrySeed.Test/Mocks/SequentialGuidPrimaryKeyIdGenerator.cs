using CherrySeed.PrimaryKeyIdGeneration;
using CherrySeed.Test.Infrastructure;

namespace CherrySeed.Test.Mocks
{
    public class SequentialGuidPrimaryKeyIdGenerator : IPrimaryKeyIdGenerator
    {
        private int _id = 0;

        public object Generate()
        {
            _id++;
            return _id.ToGuid();
        }
    }
}