using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.Configuration;
using CherrySeed.EntitySettings;
using CherrySeed.IdMappings;
using CherrySeed.ObjectTransformation;
using CherrySeed.TypeTransformations;
using CherrySeed.Utils;

namespace CherrySeed
{
    public interface ICherrySeeder
    {
        void Seed();
    }

    public class CherrySeeder : ICherrySeeder
    {
        private readonly SeederConfigurationBuilder _configBuilder;
        private readonly Dictionary<Type, EntityMetadata> _entityMetadataDict;
        private readonly Dictionary<Type, EntitySetting> _entitySettings;

        public CherrySeeder(Action<ISeederConfigurationBuilder> configurationExpression)
        {
            // init
            _configBuilder = new SeederConfigurationBuilder();
            _entityMetadataDict = new Dictionary<Type, EntityMetadata>();
            _entitySettings = new Dictionary<Type, EntitySetting>();

            configurationExpression(_configBuilder);

            var entitySettings = _configBuilder.EntitySettings;
            foreach (var objectDescription in entitySettings)
            {
                _entitySettings.Add(objectDescription.EntityType, objectDescription);
            }
        }

        public void Seed()
        {
            if (_configBuilder.DataProvider == null)
            {
                throw new InvalidOperationException("_entityDataProvider is not set");
            }

            var idMappingProvider = new IdMappingProvider();

            var objectListTransformation = new ObjectListTransformation(
                new ObjectTransformation.ObjectTransformation(
                    new TypeTransformationProvider(_configBuilder.TypeTransformations),
                    idMappingProvider));

            foreach (var entitySettingPair in _entitySettings.OrderBy(es => es.Value.Order))
            {
                var entityType = entitySettingPair.Key;
                var entitySetting = entitySettingPair.Value;

                entitySetting.PrimaryKey.FinalPrimaryKeyName = GetFinalPrimaryKeyName(entityType, entitySetting.PrimaryKey.PrimaryKeyNames);

                _entityMetadataDict.Add(entityType, new EntityMetadata
                {
                    //EntityName = entityType.FullName,
                    EntityType = entityType,
                    EntitySetting = entitySetting
                });
            }

            if (_configBuilder.IsClearBeforeSeedingEnabled)
            {
                foreach (var entityMetadataPair in _entityMetadataDict.OrderByDescending(em => em.Value.EntitySetting.Order))
                {
                    var entityMetadata = entityMetadataPair.Value;
                    var repository = entityMetadata.EntitySetting.Repository;

                    repository.RemoveEntities(entityMetadata.EntityType);
                }
            }

            var entityData = _configBuilder.DataProvider.GetEntityData();

            foreach (var objectMetadataPair in _entityMetadataDict.OrderBy(em => em.Value.EntitySetting.Order))
            {
                var entityType = objectMetadataPair.Key;
                var entityMetadata = objectMetadataPair.Value;

                entityMetadata.ObjectsAsDict =
                    entityData.First(od => entityMetadata.EntitySetting.EntityNames.Contains(od.EntityName)).Objects;

                var entitySetting = entityMetadata.EntitySetting;
                var createEntityTarget = entitySetting.Repository;

                entityMetadata.Objects = Transform(entityMetadata.EntityType, entityMetadata.ObjectsAsDict,
                    objectListTransformation, entitySetting);

                for (var i = 0; i < entityMetadata.Objects.Count; i++)
                {
                    var obj = entityMetadata.Objects[i];
                    var objDict = entityMetadata.ObjectsAsDict[i];

                    _configBuilder.AfterTransformationAction?.Invoke(objDict, obj);

                    createEntityTarget.SaveEntity(obj);

                    var entityIdInRepo = ReflectionUtil.GetPropertyValue(obj, entityMetadata.EntityType,
                        entitySetting.PrimaryKey.FinalPrimaryKeyName);

                    var entityIdInProvider = GetProviderIdOfObject(objDict, entitySetting.PrimaryKey.FinalPrimaryKeyName);
                    idMappingProvider.SetIdMapping(entityMetadata.EntityType, entityIdInProvider, entityIdInRepo);
                }
            }
        }

        private string GetFinalPrimaryKeyName(Type type, List<string> availablePropertyNames)
        {
            foreach (var propertyName in availablePropertyNames)
            {
                var propertyNameWithoutTokens = propertyName.Replace("{ClassName}", type.Name);

                if (ReflectionUtil.ExistProperty(type, propertyNameWithoutTokens))
                {
                    return propertyNameWithoutTokens;
                }
            }

            throw new InvalidOperationException("Property not found");
        }

        private string GetProviderIdOfObject(Dictionary<string, string> objectDict, string primaryKeyName)
        {
            var providerId = objectDict[primaryKeyName];
            return providerId;
        }

        public List<object> Transform(Type type, List<Dictionary<string, string>> inputObjectDictionary,
            ObjectListTransformation objectListTransformation, EntitySetting entitySetting)
        {
            return objectListTransformation.Transform(type, inputObjectDictionary, entitySetting);
        }
    }
}