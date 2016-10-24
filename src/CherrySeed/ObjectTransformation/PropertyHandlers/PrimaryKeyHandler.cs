using System;
using CherrySeed.EntitySettings;
using CherrySeed.Utils;

namespace CherrySeed.ObjectTransformation.PropertyHandlers
{
    class PrimaryKeyHandler : IPropertyHandler
    {
        public static bool CanHandle(string propertyName, EntitySetting entitySetting)
        {
            var isPrimaryKey = propertyName == entitySetting.PrimaryKey.PrimaryKeyName;

            // property should be primary key
            if (!isPrimaryKey)
                return false;

            // generator should be enabled
            if (!entitySetting.IdGeneration.IsGeneratorEnabled)
                return false;

            // database generator should be disabled
            if (entitySetting.IdGeneration.IsDatabaseGenerationEnabled)
                return false;

            return true;
        }

        public void Handle(object obj, string propertyName, string propertyValue, EntitySetting entitySetting)
        {
            object primaryKeyId = null;

            try
            {
                primaryKeyId = entitySetting.IdGeneration.Generator.Generator.Generate();
                ReflectionUtil.SetProperty(obj, propertyName, primaryKeyId);
            }
            catch (Exception ex)
            {
                throw new PrimaryKeyException(obj.GetType(), propertyName, primaryKeyId, ex);
            }
        }
    }
}