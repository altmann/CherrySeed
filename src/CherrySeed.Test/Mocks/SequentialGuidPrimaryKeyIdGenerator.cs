using CherrySeed.PrimaryKeyIdGeneration;
using CherrySeed.Test.Convert;

namespace CherrySeed.Test.Mocks
{
    public class SequentialGuidPrimaryKeyIdGenerator : IPrimaryKeyIdGenerator
    {
        private int _id = 0;

        public object Generate()
        {
            _id++;
            return Converter.ToGuid(_id);
        }
    }
}