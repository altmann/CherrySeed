using CherrySeed.EntitySettings;

namespace CherrySeed.ObjectTransformation.PropertyHandlers
{
    interface IPropertyHandler
    {
        void Handle(object obj, string propertyName, string propertyValue, EntitySetting entitySetting);
    }
}