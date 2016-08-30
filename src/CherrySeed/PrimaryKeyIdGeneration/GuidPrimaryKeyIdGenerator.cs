using System;

namespace CherrySeed.PrimaryKeyIdGeneration
{
    class GuidPrimaryKeyIdGenerator : IPrimaryKeyIdGenerator
    {
        public object Generate()
        {
            return Guid.NewGuid();
        }
    }
}