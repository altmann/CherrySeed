using CherrySeed.PrimaryKeyIdGeneration;

namespace CherrySeed.EntitySettings
{
    public class IdGenerationSetting
    {
        public IdGenerationSetting()
        { }

        public IdGenerationSetting(IPrimaryKeyIdGenerator generator)
        {
            Generator = generator;
        }

        public IPrimaryKeyIdGenerator Generator { get; set; }
    }
}