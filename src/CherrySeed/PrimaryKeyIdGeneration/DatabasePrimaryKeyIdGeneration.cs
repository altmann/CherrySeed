namespace CherrySeed.PrimaryKeyIdGeneration
{
    public class DatabasePrimaryKeyIdGeneration : IMainPrimaryKeyIdGeneration
    {
        public IPrimaryKeyIdGenerator Generator { get; set; }
    }
}