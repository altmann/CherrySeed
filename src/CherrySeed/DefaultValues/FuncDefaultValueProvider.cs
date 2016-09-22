using System;

namespace CherrySeed.DefaultValues
{
    class FuncDefaultValueProvider : IDefaultValueProvider
    {
        private readonly Func<object> _defaultValueFunc;

        public FuncDefaultValueProvider(Func<object> defaultValueFunc)
        {
            _defaultValueFunc = defaultValueFunc;
        }

        public object GetDefaultValue()
        {
            return _defaultValueFunc();
        }
    }
}