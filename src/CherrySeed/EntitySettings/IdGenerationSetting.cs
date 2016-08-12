using CherrySeed.IdGeneration;

namespace CherrySeed.EntitySettings
{
    public class IdGenerationSetting
    {
        public IdGenerationSetting()
        { }

        public IdGenerationSetting(IIdGenerator generator)
        {
            Generator = generator;
        }

        public IIdGenerator Generator { get; set; }
    }
}