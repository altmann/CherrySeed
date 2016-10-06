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
        private readonly SeederConfigurationBuilder _configBuilder;
        private readonly Dictionary<Type, EntityMetadata> _entityMetadataDict;
        private readonly IdMappingProvider _idMappingProvider;
        private readonly ObjectListTransformation _objectListTransformation;

        public CherrySeeder(Action<ISeederConfigurationBuilder> configurationExpression)
        {
            // init
            _configBuilder = new SeederConfigurationBuilder();
            _entityMetadataDict = new Dictionary<Type, EntityMetadata>();
            _idMappingProvider = new IdMappingProvider();

            configurationExpression(_configBuilder);

            _objectListTransformation = new ObjectListTransformation(
                new ObjectTransformation.ObjectTransformation(
                    new TypeTransformationProvider(_configBuilder.TypeTransformations),
                    _idMappingProvider));
            
            foreach (var entitySetting in _configBuilder.EntitySettings)
            {
                var entityType = entitySetting.EntityType;

                entitySetting.PrimaryKey.FinalPrimaryKeyName = GetFinalPrimaryKeyName(entityType, entitySetting.PrimaryKey.PrimaryKeyNames);

                _entityMetadataDict.Add(entityType, new EntityMetadata
                {
                    EntityType = entityType,
                    EntitySetting = entitySetting
                });
            }

            if (_configBuilder.DataProvider == null)
            {
                throw new MissingConfigurationException("DataProvider");
            }
            if (_configBuilder.DefaultRepository == null)
            {
                throw new MissingConfigurationException("Repository");
            }
        }

        public void Seed()
        {
            if (_configBuilder.IsClearBeforeSeedingEnabled)
            {
                Clear();
            }

            var entityDataList = _configBuilder.DataProvider.GetEntityDataList();

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

                _configBuilder.BeforeSaveAction?.Invoke(objDict, obj);

                createEntityTarget.SaveEntity(obj);

                entitySetting.AfterSave(obj);
                _configBuilder.AfterSaveAction?.Invoke(objDict, obj);

                var entityIdInRepo = ReflectionUtil.GetPropertyValue(obj, entityMetadata.EntityType,
                    entitySetting.PrimaryKey.FinalPrimaryKeyName);

                var entityIdInProvider = GetProviderIdOfObject(objDict, entitySetting.PrimaryKey.FinalPrimaryKeyName);
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

        private List<object> Transform(Type type, List<Dictionary<string, string>> inputObjectDictionary,
            ObjectListTransformation objectListTransformation, EntitySetting entitySetting)
        {
            return objectListTransformation.Transform(type, inputObjectDictionary, entitySetting);
        }
    }
}