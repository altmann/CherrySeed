using CherrySeed.IdGeneration;

namespace CherrySeed.EntitySettings
{
    public class CodeIdGenerationSettingBuilder
    {
        private readonly IdGenerationSetting _idGenerationSetting;

        public CodeIdGenerationSettingBuilder()
        {
            _idGenerationSetting = new IdGenerationSetting();
        }

        public CodeIdGenerationSettingBuilder WithIntegerIdGenerator()
        {
            _idGenerationSetting.Generator = new IntegerIdGenerator();
            return this;
        }

        public CodeIdGenerationSettingBuilder WithGuidIdGenerator()
        {
            _idGenerationSetting.Generator = new GuidIdGenerator();
            return this;
        }

        public CodeIdGenerationSettingBuilder WithCustomIdGenerator(IIdGenerator generator)
        {
            _idGenerationSetting.Generator = generator;
            return this;
        }
    }
}