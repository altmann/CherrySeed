namespace CherrySeed.PrimaryKeyIdGeneration
{
    class StringPrimaryKeyIdGenerator : IPrimaryKeyIdGenerator
    {
        private readonly string _prefix;
        private readonly IntegerPrimaryKeyIdGenerator _primaryKeyIdGenerator;

        public StringPrimaryKeyIdGenerator(string prefix, int startId, int steps)
        {
            _prefix = prefix;
            _primaryKeyIdGenerator = new IntegerPrimaryKeyIdGenerator(startId, steps);
        }

        public object Generate()
        {
            return $"{_prefix}{_primaryKeyIdGenerator.Generate()}";
        }
    }
}