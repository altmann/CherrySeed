namespace CherrySeed.PrimaryKeyIdGeneration
{
    public class IntegerPrimaryKeyIdGenerator : IPrimaryKeyIdGenerator
    {
        private int _id;
        private readonly int _steps;

        public IntegerPrimaryKeyIdGenerator(int startId, int steps)
        {
            _id = startId;
            _steps = steps;
        }

        public object Generate()
        {
            var result = _id;

            _id = _id + _steps;

            return result;
        }
    }
}