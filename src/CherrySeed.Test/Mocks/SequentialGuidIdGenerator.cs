using CherrySeed.IdGeneration;
using CherrySeed.Test.Convert;

namespace CherrySeed.Test.Mocks
{
    public class SequentialGuidIdGenerator : IIdGenerator
    {
        private int _id = 0;

        public object Generate()
        {
            _id++;
            return Converter.ToGuid(_id);
        }
    }
}