using CherrySeed.PrimaryKeyIdGeneration;

namespace CherrySeed.EntitySettings
{
    public class IdGenerationSetting
    {
        public IMainPrimaryKeyIdGeneration Generator { get; set; }
        public bool IsGeneratorEnabled => Generator != null;
        public bool IsDatabaseGenerationEnabled => Generator is DatabasePrimaryKeyIdGeneration;
        public bool IsApplicationGenerationEnabled => Generator is ApplicationPrimaryKeyIdGeneration;
    }
}