using CherrySeed.PrimaryKeyIdGeneration;

namespace CherrySeed.Test.Mocks
{
    public class SequentialStringPrimaryKeyIdGenerator : IPrimaryKeyIdGenerator
    {
        private readonly string _prefix;
        private int _id = 0;

        public SequentialStringPrimaryKeyIdGenerator(string prefix)
        {
            _prefix = prefix;
        }

        public object Generate()
        {
            _id++;
            return _prefix + _id;
        }
    }
}