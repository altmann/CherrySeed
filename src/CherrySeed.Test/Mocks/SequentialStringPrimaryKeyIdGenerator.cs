using CherrySeed.PrimaryKeyIdGeneration;

namespace CherrySeed.Test.Mocks
{
    public class SequentialStringPrimaryKeyIdGenerator : IPrimaryKeyIdGenerator
    {
        private int _id = 0;

        public object Generate()
        {
            _id++;
            return "CUSTOM" + _id.ToString();
        }
    }
}