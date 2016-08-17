using System;

namespace CherrySeed.PrimaryKeyIdGeneration
{
    public class GuidPrimaryKeyIdGenerator : IPrimaryKeyIdGenerator
    {
        public object Generate()
        {
            return Guid.NewGuid();
        }
    }
}