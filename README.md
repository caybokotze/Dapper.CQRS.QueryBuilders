# DapperDoodle

Created: Dec 22, 2020 7:12 AM

# Description

> Dapper doodle is a CQRS wrapper for dapper. The idea behind dapper doodle is to get up and running with dapper quickly, by providing a set of base classes and interfaces that can assist you to get up and running a lot quicker.

# WTF is Dapper

Dapper is a lightweight micro-ORM developed by Stack-Overflow as an alternative to entity framework. A developer at stack overflow built it to solve the issue they were having with Entity Frameworks bulky slow queries. It's almost as quick as ADO.NET but easier to use and map objects against each other.

# What is CQRS?

CQRS stands for Command Query Responsibility Segregation. The idea behind it is to be able to separate your commands (DML) and your queries (DQL). CQRS is a great pattern to follow for small or large systems and offers the flexibility to keep all your database interactions very structured and orderly. It is quite similar to the repository pattern, however instead of using interfaces as abstractions we are abstracting every database transaction as a class instead of the context oriented model.

# Getting Started

To get started you need to go and download the main DapperDoodle nuget package from nuget.org. The package is called DapperDoodle if you would prefer to download it directly from Nuget. Or you can download the .dll here and import the dependency directly into your project.

After creating a [ASP.NET](http://asp.NET) Web app you can follow these steps to hook up dapper doodle to your project.

## Startup.cs

Below, you can register DapperDoodle, which will setup all the required dependencies for DapperDoodle to work. The `ConfigureDapperDoodle()` method has support for 2 parameters, the connection string and the Database type.

Note: You can only use the null parameter with the DBMS.SQLite option which will create a default connection string on your behalf. All other database types will throw a Argument Null Exception.

```csharp
public void ConfigureServices(IServiceCollection services)
{
		services.AddControllers();
    services.ConfigureDapperDoodle(null, DBMS.SQLite);
}
```

Now you are ready to get started using the CQRS setup within the project. All you have to do now is to start creating your command and query classes and plug them into your controllers.

## Commands

When calling the Execute() method, you do need to supply at least 1 parameter. Otherwise you end up with a recursive call, in this case it will lead to your app crashing real quick.

### Non-Generic

Non-Generic 

```csharp
public class PersistSomething : Command
{
    public override void Execute()
    {
        Execute("INSERT INTO People(name, surname) VALUES ('John', 'Williams');");
    }
}
```

### Generic

With Generic implementations you can define the expected return type in the base class. For commands this will commonly only be an integer in use cases where retrieving the ID of the inserted record is required.

```csharp
public class PersistSomething : Command<int>
{
    public override void Execute()
    {
        var returnId = 
                Execute("INSERT INTO People(name, surname) VALUES ('John', 'Williams'); SELECT last_insert_id();");
    }
}
```

### Using the Command Builder

---

> The command builder is a set of extensions build into dapper doodle to allow you to be able to build up SQL queries a bit faster using CQRS, for the common queries where writing them manually might not be necessary.

When using the command builder, there is no need to define any generics for the command class as they are defined in the command builder.

```csharp
public class PersistSomething : Command
{
    public override void Execute()
    {
        var id = InsertAndReturnId("INSERT INTO People(name, surname) VALUES ('John', 'Williams');");
    }
}
```

**Insert Builder**

The insert builder is overloaded with update by default, so if the record does already exist, it will just update the information, not insert it again, which will most likely lead to an exception.

The insert builder returns the id of the created record, which can be persisted.

```csharp
public override void Execute()
{
    var id = BuildInsert<Person>();
}
```

**Update Builder**

```csharp
public override void Execute()
{
    var id = BuildUpdate<Person>();
}
```

**Person Model**

```csharp
public class Person
{
    public string Name { get; set; }
    public string Surname { get; set; }
}
```

## Queries

# Extension Libraries

Core library is built with .NET Standard 2.1. However currently the main package dependencies are for:

- Dapper (2.0.78)
- Microsoft.Exentions.DependencyInjection (5.0.1)
- MySql.Data (8.0.22)
- PluralizeService.Core (1.2.19)
- Microsoft.Data.Sqlite (5.0.1)

# Database Support

- SQLite
- MySQL

# Feature List

- [ ]  Command Builder
- [ ]  Query Builder
- [ ]  Auto-Insert Statements
- [ ]  Auto-Update Statements
- [ ]  Auto-Select Statements
- [ ]  CQRS Interface helpers
