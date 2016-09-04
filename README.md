# What is CherrySeed?
CherrySeed is a simple little .NET library built to solve a common problem - create and seed test data into the database. This type of code is boring to write and bring relations under control is hard, so why not use a library for that?

# Why use CherrySeed?
- Bring relations between entities under control with an easy-to-use API 
- High separation of concerns
  - Data providers (defining test data via csv, json, xml, etc.)
  - Repository (storing test data via O/R mapping framework to your choice)
- Some ready-to-use data providers and repositories
- High extensibility
    - Custom data providers
    - Custom repositories
    - Custom type transformations
    - Extension points
- Many types are supported out of the box (Integer, string, enum, Nullable types, etc.)

# How do I use CherrySeed?

    var config = new CherrySeedConfiguration(cfg =>
    {
        // set data provider
        cfg.WithDataProvider(new CsvDataProvider());

        // set repository
        cfg.WithGlobalRepository(new EfRepository());

        // set entity specific settings
        cfg.ForEntity<Person>()
            .WithPrimaryKey(e => e.Identification);
    });

    var cherrySeeder = config.CreateSeeder();
    cherrySeeder.Seed();

More see in [Getting Started](https://github.com/altmann/CherrySeed/wiki/Getting-Started).

# Resources
- [Install via NuGet](https://www.nuget.org/packages?q=CherrySeed)
- [Documentation](https://github.com/altmann/CherrySeed/wiki/Getting-Started)
