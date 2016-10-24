using System;
using System.Linq;
using CherrySeed.EntitySettings;
using CherrySeed.IdMappings;
using CherrySeed.Utils;

namespace CherrySeed.ObjectTransformation.PropertyHandlers
{
    class ForeignKeyHandler : IPropertyHandler
    {
        private readonly IdMappingProvider _idMappingProvider;

        public ForeignKeyHandler(IdMappingProvider idMappingProvider)
        {
            _idMappingProvider = idMappingProvider;
        }

        public static bool CanHandle(string propertyName, EntitySetting entitySetting)
        {
            return entitySetting.References.Select(rd => rd.ReferenceName).Contains(propertyName);
        }

        public void Handle(object obj, string propertyName, string propertyValue, EntitySetting entitySetting)
        {
            try
            {
                var propertyType = ReflectionUtil.GetPropertyType(obj.GetType(), propertyName);

                var referenceSetting = entitySetting.References.First(rd => rd.ReferenceName == propertyName);
                var foreignKeyId = _idMappingProvider.GetRepositoryId(referenceSetting.ReferenceType, propertyValue);

                if (ReflectionUtil.IsReferenceType(propertyType))
                {
                    var referenceModel = entitySetting.Repository.LoadEntity(propertyType, foreignKeyId);
                    ReflectionUtil.SetProperty(obj, propertyName, referenceModel);
                }
                else
                {
                    ReflectionUtil.SetProperty(obj, propertyName, foreignKeyId);
                }
            }
            catch (Exception ex)
            {
                throw new ForeignKeyException(obj.GetType(), propertyName, propertyValue, ex);
            }
        }
    }
}