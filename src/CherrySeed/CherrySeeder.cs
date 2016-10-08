using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.Configuration;
using CherrySeed.Configuration.Exceptions;
using CherrySeed.EntityDataProvider;
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
        void Clear();
    }

    public class CherrySeeder : ICherrySeeder
    {
        private readonly Dictionary<Type, EntityMetadata> _entityMetadataDict;
        private readonly IdMappingProvider _idMappingProvider;
        private readonly ObjectListTransformation _objectListTransformation;
        private readonly SeederConfigurationValidator _configurationValidator;
        private readonly SeederConfiguration _configuration;

        public CherrySeeder(Action<ISeederConfigurationBuilder> configurationExpression)
        {
            // init
            _entityMetadataDict = new Dictionary<Type, EntityMetadata>();
            _idMappingProvider = new IdMappingProvider();
            _configurationValidator = new SeederConfigurationValidator();

            var configBuilder = new SeederConfigurationBuilder();
            configurationExpression(configBuilder);
            _configuration = configBuilder.Build();

            _objectListTransformation = new ObjectListTransformation(
                new ObjectTransformation.ObjectTransformation(
                    new TypeTransformationProvider(_configuration.TypeTransformations),
                    _idMappingProvider));
            
            foreach (var entitySetting in _configuration.EntitySettings)
            {
                var entityType = entitySetting.EntityType;

                entitySetting.PrimaryKey.PrimaryKeyName = GetFinalPrimaryKeyName(entityType, entitySetting.PrimaryKey.PrimaryKeyName, _configuration.DefaultPrimaryKeyNames);

                _entityMetadataDict.Add(entityType, new EntityMetadata
                {
                    EntityType = entityType,
                    EntitySetting = entitySetting
                });
            }

            _configurationValidator.IsValid(_configuration);
        }

        public void Seed()
        {
            if (_configuration.IsClearBeforeSeedingEnabled)
            {
                Clear();
            }

            var entityDataList = _configuration.DataProvider.GetEntityDataList();

            foreach (var objectMetadataPair in _entityMetadataDict.OrderBy(em => em.Value.EntitySetting.Order))
            {
                ProcessEntity(objectMetadataPair, entityDataList);
            }
        }

        private void ProcessEntity(KeyValuePair<Type, EntityMetadata> objectMetadataPair, List<EntityData> entityData)
        {
            var entityType = objectMetadataPair.Key;
            var entityMetadata = objectMetadataPair.Value;

            var eData = entityData.SingleOrDefault(od => entityMetadata.EntitySetting.EntityNames.Contains(od.EntityName));

            if (eData == null)
            {
                return;
            }

            entityMetadata.ObjectsAsDict = eData.Objects;

            var entitySetting = entityMetadata.EntitySetting;
            var createEntityTarget = entitySetting.Repository;

            entityMetadata.Objects = Transform(entityMetadata.EntityType, entityMetadata.ObjectsAsDict,
                _objectListTransformation, entitySetting);

            for (var i = 0; i < entityMetadata.Objects.Count; i++)
            {
                var obj = entityMetadata.Objects[i];
                var objDict = entityMetadata.ObjectsAsDict[i];

                _configuration.BeforeSaveAction?.Invoke(objDict, obj);

                createEntityTarget.SaveEntity(obj);

                entitySetting.AfterSave(obj);
                _configuration.AfterSaveAction?.Invoke(objDict, obj);

                var entityIdInRepo = ReflectionUtil.GetPropertyValue(obj, entityMetadata.EntityType,
                    entitySetting.PrimaryKey.PrimaryKeyName);

                var entityIdInProvider = GetProviderIdOfObject(objDict, entitySetting.PrimaryKey.PrimaryKeyName);
                _idMappingProvider.SetIdMapping(entityMetadata.EntityType, entityIdInProvider, entityIdInRepo);
            }
        }

        public void Clear()
        {
            foreach (var entityMetadataPair in _entityMetadataDict.OrderByDescending(em => em.Value.EntitySetting.Order))
            {
                var entityMetadata = entityMetadataPair.Value;
                var repository = entityMetadata.EntitySetting.Repository;

                repository.RemoveEntities(entityMetadata.EntityType);
            }
        }

        private string GetFinalPrimaryKeyName(Type type, string primaryKeyName, List<string> defaultPrimaryKeyNames)
        {
            if (!string.IsNullOrEmpty(primaryKeyName))
            {
                return primaryKeyName;
            }

            foreach (var propertyName in defaultPrimaryKeyNames)
            {
                var propertyNameWithoutTokens = propertyName.Replace("{ClassName}", type.Name);

                if (ReflectionUtil.ExistProperty(type, propertyNameWithoutTokens))
                {
                    return propertyNameWithoutTokens;
                }
            }

            return null;
        }

        private string GetProviderIdOfObject(Dictionary<string, string> objectDict, string primaryKeyName)
        {
            var providerId = objectDict[primaryKeyName];
            return providerId;
        }

        private List<object> Transform(Type type, List<Dictionary<string, string>> inputObjectDictionary,
            ObjectListTransformation objectListTransformation, EntitySetting entitySetting)
        {
            return objectListTransformation.Transform(type, inputObjectDictionary, entitySetting);
        }
    }
}