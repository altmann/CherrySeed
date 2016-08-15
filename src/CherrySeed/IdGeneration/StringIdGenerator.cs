namespace CherrySeed.IdGeneration
{
    public class StringIdGenerator : IIdGenerator
    {
        private readonly string _prefix;
        private readonly IntegerIdGenerator _idGenerator;

        public StringIdGenerator(string prefix, int startId, int steps)
        {
            _prefix = prefix;
            _idGenerator = new IntegerIdGenerator(startId, steps);
        }

        public object Generate()
        {
            return $"{_prefix}{_idGenerator.Generate()}";
        }
    }
}