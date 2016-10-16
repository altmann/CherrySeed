using CherrySeed.PrimaryKeyIdGeneration;

namespace CherrySeed.EntitySettings
{
    public class IdGenerationSetting
    {
        public IdGenerationSetting()
        {
            IsGeneratorEnabled = true;
        }

        public bool IsGeneratorEnabled { get; set; }
        public IPrimaryKeyIdGenerator Generator { get; set; }
        public bool IsDatabaseGenerationEnabled => Generator == null;
    }
}