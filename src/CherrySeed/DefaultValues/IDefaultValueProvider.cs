namespace CherrySeed.DefaultValues
{
    public interface IDefaultValueProvider
    {
        object GetDefaultValue();
    }

    class ConstantDefaultValueProvider<T> : IDefaultValueProvider
    {
        private readonly T _value;

        public ConstantDefaultValueProvider(T value)
        {
            _value = value;
        }

        public object GetDefaultValue()
        {
            return _value;
        }
    }
}