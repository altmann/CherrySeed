using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.Configuration;
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
        private readonly SeederConfiguration _configuration;

        public CherrySeeder(Action<ISeederConfigurationBuilder> configurationExpression)
        {
            _entityMetadataDict = new Dictionary<Type, EntityMetadata>();
            _idMappingProvider = new IdMappingProvider();

            var configBuilder = new SeederConfigurationBuilder();
            configurationExpression(configBuilder);
            _configuration = configBuilder.Build();

            _objectListTransformation = new ObjectListTransformation(
                new ObjectTransformation.ObjectTransformation(
                    new TypeTransformationProvider(_configuration.TypeTransformations),
                    _idMappingProvider));
            
            foreach (var entitySetting in _configuration.EntitySettings)
            {
                _entityMetadataDict.Add(entitySetting.EntityType, new EntityMetadata
                {
                    EntityType = entitySetting.EntityType,
                    EntitySetting = entitySetting
                });
            }
        }

        public IDataProvider DataProvider => _configuration.DataProvider;

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

        private void ProcessEntity(KeyValuePair<Type, EntityMetadata> objectMetadataPair, List<EntityData> entityDataList)
        {
            var entityType = objectMetadataPair.Key;
            var entityMetadata = objectMetadataPair.Value;

            var eData = entityDataList.SingleOrDefault(od => entityMetadata.EntitySetting.EntityNames.Contains(od.EntityName));

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