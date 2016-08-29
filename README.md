# What is CherrySeed?
CherrySeed is a simple little library built to solve a common problem - create and seed test data into the database. This type of code is boring to write and bring relations under control is hard, so why not use a library for that?

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

CherrySeed require five things to work properly:
- Entity Class
- Entity Settings
- Test Data
- Data Provider
- Repository

## Entity Class
The entity class defines entity name and entity field names. Commonly you don’t have to create this entity class specifically for CherrySeed because you already use it in your application. 

    public class Person
    {
        public int Identification { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
    }

## Entity Settings
With entity settings metadata is added to the entity. You define which field is the primary key, which fields are foreign keys, etc. More info see [Entity Settings](./Entity-Settings).

## Test Data
Test data is the data which is inserted into the database. For example the entity Person with Name “Julia” and Birthdate 1990/01/01. This is your main work to create valid and good test data. More info see [Defining Test Data](./Defining-Test-Data).

## Data Provider
The data provider defines the way how to load and define test data. If test data comes from a csv/json/xml file or is declared directly in C# code is up to the data provider. More info see [Data Provider](./Data-Provider).

## Repository
The repository is responsible for storing test data in the database. If Entity Framework, Hibernate or another O/R mapping framework is used is up to the repository. More info see [Repository](./Repository).

# Release and Contact
Now this library is in alpha state, but in a few weeks I will release it. Drop me a line via twitter [@michael_altmann](https://twitter.com/michael_altmann) if you are interested in this library.
