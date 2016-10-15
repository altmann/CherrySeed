using System;
using System.Collections.Generic;
using CherrySeed.Configuration;
using CherrySeed.EntityDataProvider;
using CherrySeed.Repositories;
using CherrySeed.Test.Mocks;

namespace CherrySeed.Test.Infrastructure
{
    public class CherrySeedDriver
    {
        private ICherrySeeder _cherrySeeder;

        public void InitAndSeed(List<EntityData> data, IRepository repository,
           Action<ISeederConfigurationBuilder> entitySettings)
        {
            var config = new CherrySeedConfiguration(cfg =>
            {
                cfg.WithDataProvider(new DictionaryDataProvider(data));
                cfg.WithRepository(repository);

                entitySettings(cfg);
            });

            var cherrySeeder = config.CreateSeeder();
            cherrySeeder.Seed();
        }

        public void InitAndSeed(IDataProvider dataProvider, IRepository repository,
            Action<ISeederConfigurationBuilder> entitySettings)
        {
            var config = new CherrySeedConfiguration(cfg =>
            {
                cfg.WithDataProvider(dataProvider);
                cfg.WithRepository(repository);

                entitySettings(cfg);
            });

            _cherrySeeder = config.CreateSeeder();
            _cherrySeeder.Seed();
        }

        public void Clear()
        {
            _cherrySeeder.Clear();
        }
    }
}