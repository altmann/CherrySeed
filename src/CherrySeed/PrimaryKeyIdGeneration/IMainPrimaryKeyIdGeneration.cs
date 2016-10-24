namespace CherrySeed.PrimaryKeyIdGeneration
{
    public interface IMainPrimaryKeyIdGeneration
    {
        IPrimaryKeyIdGenerator Generator { get; set; }
    }
}