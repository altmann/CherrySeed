namespace CherrySeed.PrimaryKeyIdGeneration
{
    public class ApplicationPrimaryKeyIdGeneration : IMainPrimaryKeyIdGeneration
    {
        public IPrimaryKeyIdGenerator Generator { get; set; }
    }
}