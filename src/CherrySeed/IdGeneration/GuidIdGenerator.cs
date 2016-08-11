using System;

namespace CherrySeed.IdGeneration
{
    public class GuidIdGenerator : IIdGenerator
    {
        public object Generate()
        {
            return Guid.NewGuid();
        }
    }
}