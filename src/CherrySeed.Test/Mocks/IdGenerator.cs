using System;

namespace CherrySeed.Test.Mocks
{

    public class IntegerIdGenerator
    {
        
    }

    public class GuidIdGenerator
    {
        private int _id = 1;

        public Guid Generate()
        {
            _id += 1;
            return ToGuid(_id);
        }

        private Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}