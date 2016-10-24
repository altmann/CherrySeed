using System;

namespace CherrySeed.PrimaryKeyIdGeneration.ApplicationGeneration
{
    class GuidPrimaryKeyIdGenerator : IPrimaryKeyIdGenerator
    {
        public object Generate()
        {
            return Guid.NewGuid();
        }
    }
}