using System;
using System.Collections.Generic;
using CherrySeed.EntityDataProvider;
using CherrySeed.EntitySettings;
using CherrySeed.PrimaryKeyIdGeneration;
using CherrySeed.Repositories;
using CherrySeed.TypeTransformations;

namespace CherrySeed.Configuration
{
    public interface ISeederConfigurationBuilder
    {
        IEntitySettingBuilder<T> ForEntity<T>();
        void WithDataProvider(IDataProvider dataProvider);
        void AddTypeTransformation(Type type, ITypeTransformation transformation);
        void WithDefaultPrimaryKeyNames(params string[] primaryKeyNames);
        void WithRepository(IRepository repository);
        void DisableClearBeforeSeeding();
        void BeforeSave(Action<Dictionary<string, string>, object> beforeSaveAction);
        void AfterSave(Action<Dictionary<string, string>, object> afterSaveAction);
        void WithPrimaryKeyIdGenerationInApplicationAsInteger(int startId = 1, int steps = 1);
        void WithPrimaryKeyIdGenerationInApplicationAsGuid();
        void WithPrimaryKeyIdGenerationInApplicationAsString(string prefix = "", int startId = 1, int steps = 1);
        void WithCustomPrimaryKeyIdGenerationInApplication(IPrimaryKeyIdGenerator generator);
        void WithEmptyStringMarker(string marker);
    }
}