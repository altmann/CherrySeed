using CherrySeed.DefaultValues;

namespace CherrySeed.Test.Mocks
{
    public class DefaultValueProviderMock<T> : IDefaultValueProvider
    {
        private readonly T _value;

        public DefaultValueProviderMock(T value)
        {
            _value = value;
        }

        public object GetDefaultValue()
        {
            return _value;
        }
    }
}