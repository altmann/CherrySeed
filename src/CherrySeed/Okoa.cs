using System;
using System.Collections.Generic;
using System.Linq;
using CherrySeed.EntityDefinitions;
using CherrySeed.EntitySettings;
using CherrySeed.EntityTargets;
using CherrySeed.IdMappings;
using CherrySeed.ObjectTransformation;
using CherrySeed.SimpleTypeTransformations;
using CherrySeed.Utils;

namespace CherrySeed
{
    public class EntityMetadata
    {
        public string EntityName { get; set; }
        public Type EntityType { get; set; }
        public List<Dictionary<string, string>> ObjectsAsDict { get; set; }
        public List<object> Objects { get; set; }
        public EntitySetting EntitySetting { get; set; }
    }

    public class Okoa
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

        public ICreateEntityTarget DefaultCreateEntityTarget
        {
            get { return _defaultCompositeEntitySettingBuilder.DefaultCreateEntityTarget; }
            set { _defaultCompositeEntitySettingBuilder.DefaultCreateEntityTarget = value; }
        }

        public IRemoveEntitiesTarget DefaultRemoveEntitiesEntitiesTarget
        {
            get { return _defaultCompositeEntitySettingBuilder.DefaultRemoveEntitiesTarget; }
            set { _defaultCompositeEntitySettingBuilder.DefaultRemoveEntitiesTarget = value; }
        }

        private readonly CompositeEntitySettingBuilder _defaultCompositeEntitySettingBuilder;

        public bool IsRemoveEntitiesEnabled { get; set; }
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

        public Okoa()
        {
            _defaultCompositeEntitySettingBuilder = new CompositeEntitySettingBuilder();

            DefaultCreateEntityTarget = new EmptyTarget();
            IsRemoveEntitiesEnabled = true;
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

            if (IsRemoveEntitiesEnabled)
            {
                foreach (var entityMetadataPair in _entityMetadataDict.OrderByDescending(em => em.Value.EntitySetting.Order))
                {
                    var entityMetadata = entityMetadataPair.Value;
                    var removeEntitiesTarget = entityMetadata.EntitySetting.RemoveEntitiesTarget;

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
                var createEntityTarget = entitySetting.CreateEntityTarget;

                entityMetadata.Objects = Transform(entityMetadata.EntityType, entityMetadata.ObjectsAsDict,
                    objectListTransformation, entitySetting);

                for (var i = 0; i < entityMetadata.Objects.Count; i++)
                {
                    var obj = entityMetadata.Objects[i];
                    var objDict = entityMetadata.ObjectsAsDict[i];

                    if (AfterTransformation != null)
                        AfterTransformation(objDict, obj);

                    createEntityTarget.SaveEntity(obj);
                    var targetId = ReflectionUtil.GetPropertyValue(obj, entityMetadata.EntityType,
                        entitySetting.PrimaryKey.PrimaryKeyName);

                    var definitionId = GetDefinitionIdOfObject(objDict, entityMetadata.EntityType, obj, entitySetting);
                    idMappingProvider.SetIdMapping(entityMetadata.EntityType, definitionId, targetId);
                }
            }
        }

        private string GetDefinitionIdOfObject(Dictionary<string, string> objectDict, Type type, object obj, EntitySetting entitySetting)
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