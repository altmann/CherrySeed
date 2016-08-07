using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDefinitions;
using CherrySeed.EntitySettings;
using CherrySeed.Repositories;
using CherrySeed.IdMappings;
using CherrySeed.ObjectTransformation;
using CherrySeed.SimpleTypeTransformations;
using CherrySeed.Utils;

namespace CherrySeed
{
    public class CherrySeeder
    {
        // Configuration
        public IEntityDefinitionProvider EntityDefinitionProvider { get; set; }
        public Dictionary<Type, ISimpleTypeTransformation> SimpleTypeTransformations { get; }
        private readonly Dictionary<Type, EntitySetting> _entitySettings;

        public string DefaultPrimaryKeyName
        {
            get { return _defaultCompositeEntitySettingBuilder.DefaultPrimaryKeyName; }
            set { _defaultCompositeEntitySettingBuilder.DefaultPrimaryKeyName = value; }
        }

        public ICreateRepository DefaultCreateRepository
        {
            get { return _defaultCompositeEntitySettingBuilder.DefaultCreateRepository; }
            set { _defaultCompositeEntitySettingBuilder.DefaultCreateRepository = value; }
        }

        public IRemoveRepository DefaultRemoveRepository
        {
            get { return _defaultCompositeEntitySettingBuilder.DefaultRemoveRepository; }
            set { _defaultCompositeEntitySettingBuilder.DefaultRemoveRepository = value; }
        }

        private readonly CompositeEntitySettingBuilder _defaultCompositeEntitySettingBuilder;

        public bool IsClearBeforeSeedingEnabled { get; set; }
        public Action<Dictionary<string, string>, object> AfterTransformation { get; set; }

        public void InitEntitySettings(Action<CompositeEntitySettingBuilder> objectDescriptionExpression)
        {
            objectDescriptionExpression(_defaultCompositeEntitySettingBuilder);
            var builders = _defaultCompositeEntitySettingBuilder.ObjectDescriptionBuilders;

            var objectDescriptions = builders.Select(b => b.Build()).ToList();

            foreach (var objectDescription in objectDescriptions)
            {
                _entitySettings.Add(objectDescription.EntityType, objectDescription);
            }
        }

        private readonly Dictionary<Type, EntityMetadata> _entityMetadataDict;

        public CherrySeeder()
        {
            _defaultCompositeEntitySettingBuilder = new CompositeEntitySettingBuilder();

            DefaultCreateRepository = new EmptyRepository();
            IsClearBeforeSeedingEnabled = true;
            SimpleTypeTransformations = new Dictionary<Type, ISimpleTypeTransformation>();

            _entitySettings = new Dictionary<Type, EntitySetting>();
            _entityMetadataDict = new Dictionary<Type, EntityMetadata>();
        }

        public void StartSeeding()
        {
            if (EntityDefinitionProvider == null)
            {
                throw new InvalidOperationException("EntityDefinitionProvider is not set");
            }

            var idMappingProvider = new IdMappingProvider();

            var objectListTransformation = new ObjectListTransformation(
                new ObjectTransformation.ObjectTransformation(
                    new SimpleTypeTransformationProvider(SimpleTypeTransformations),
                    idMappingProvider));

            foreach (var entitySettingPair in _entitySettings.OrderBy(es => es.Value.Order))
            {
                var entityType = entitySettingPair.Key;
                var entitySetting = entitySettingPair.Value;

                _entityMetadataDict.Add(entityType, new EntityMetadata
                {
                    EntityName = entityType.FullName,
                    EntityType = entityType,
                    EntitySetting = entitySetting
                });
            }

            if (IsClearBeforeSeedingEnabled)
            {
                foreach (var entityMetadataPair in _entityMetadataDict.OrderByDescending(em => em.Value.EntitySetting.Order))
                {
                    var entityMetadata = entityMetadataPair.Value;
                    var removeEntitiesTarget = entityMetadata.EntitySetting.RemoveRepository;

                    removeEntitiesTarget.RemoveEntities(entityMetadata.EntityType);
                }
            }

            var objectDefinitions = EntityDefinitionProvider.GetEntityDefinitions();

            foreach (var objectMetadataPair in _entityMetadataDict.OrderBy(em => em.Value.EntitySetting.Order))
            {
                var entityType = objectMetadataPair.Key;
                var entityMetadata = objectMetadataPair.Value;

                entityMetadata.ObjectsAsDict =
                    objectDefinitions.First(od => od.EntityName == entityMetadata.EntityName).Objects;

                var entitySetting = entityMetadata.EntitySetting;
                var createEntityTarget = entitySetting.CreateRepository;

                entityMetadata.Objects = Transform(entityMetadata.EntityType, entityMetadata.ObjectsAsDict,
                    objectListTransformation, entitySetting);

                for (var i = 0; i < entityMetadata.Objects.Count; i++)
                {
                    var obj = entityMetadata.Objects[i];
                    var objDict = entityMetadata.ObjectsAsDict[i];

                    AfterTransformation?.Invoke(objDict, obj);

                    createEntityTarget.SaveEntity(obj);
                    var targetId = ReflectionUtil.GetPropertyValue(obj, entityMetadata.EntityType,
                        entitySetting.PrimaryKey.PrimaryKeyName);

                    var definitionId = GetDefinitionIdOfObject(objDict, entitySetting);
                    idMappingProvider.SetIdMapping(entityMetadata.EntityType, definitionId, targetId);
                }
            }
        }

        private string GetDefinitionIdOfObject(Dictionary<string, string> objectDict, EntitySetting entitySetting)
        {
            var primaryKeyName = entitySetting.PrimaryKey.PrimaryKeyName;
            var definitionId = objectDict[primaryKeyName];
            return definitionId;
        }

        public List<object> Transform(Type type, List<Dictionary<string, string>> inputObjectDictionary,
            ObjectListTransformation objectListTransformation, EntitySetting entitySetting)
        {
            return objectListTransformation.Transform(type, inputObjectDictionary, entitySetting);
        }
    }
}