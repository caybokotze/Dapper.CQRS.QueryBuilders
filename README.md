# Description

> Dapper doodle is a CQRS wrapper for dapper. The idea behind dapper doodle is to get up and running with dapper quickly, by providing a set of base classes and interfaces that can assist you to get up and running without much fuss, while still offering the flexibility to drop down to native SQL queries.

# WTF is Dapper?

Dapper is a lightweight micro-ORM developed by Stack-Overflow as an alternative to entity framework. A developer at stack overflow built it to solve the issue they were having with Entity Frameworks bulky slow queries. Dapper solves that by being almost as fast as [ADO.NET](http://ado.net/) but easier to use and map objects against each other. Dapper gives the developer more control by offering the ability to map objects against native SQL queries.

# WTF is CQRS?

CQRS stands for Command Query Responsibility Segregation. The idea behind it is to be able to separate your commands (DML) and your queries (DQL). CQRS is a great pattern to follow for small or large systems and offers the flexibility to keep all your database interactions very structured and orderly as the app scales. It is quite similar to the repository pattern, however instead of using interfaces as abstractions we are abstracting every database transaction as a class instead of the context-based model that is used in EF.

# Getting Started

To get started you need to go and download the main DapperDoodle nuget package from nuget.org. The package is called DapperDoodle if you would prefer to download it directly from Nuget. Or you can download the .dll here and import the dependency directly into your project.

After creating a [ASP.NET](http://asp.NET) Web app you can follow these steps to hook up dapper doodle to your project.

## Examples

If you would like to view a few examples of how to implement this in a live production application, look no further. [https://github.com/caybokotze/dapper-doodle-examples](https://github.com/caybokotze/dapper-doodle-examples)

## Startup.cs

Below, you can register DapperDoodle, which will setup all the required dependencies for DapperDoodle to work. The `ConfigureDapperDoodle()` method has support for 2 parameters, the connection string and the Database type.

> Note: You can only use the null parameter with the DBMS.SQLite option which will create a default connection string on your behalf. All other database types will throw a Argument Null Exception.

```csharp
public void ConfigureServices(IServiceCollection services)
{
		services.AddControllers();
    services.ConfigureDapperDoodle(null, DBMS.SQLite);
}
```

Now you are ready to get started using the CQRS setup within the project. All you have to do now is to start creating your command and query classes and plug them into your controllers.

## Resolving Dependencies within Controllers

Because the decencies are already setup, no need to go about registering your own manual dependencies. All you need to do is insert ICommandExecutor and IQueryExecutor into the constructor and your dependencies will resolve.

```csharp
public class HomeController : Controller
{
	public ICommandExecutor CommandExecutor { get; }
	public IQueryExecutor QueryExecutor { get; }
	
	public HomeController(
	    ICommandExecutor commandExecutor, 
	    IQueryExecutor queryExecutor)
	{
	    CommandExecutor = commandExecutor;
	    QueryExecutor = queryExecutor;
	}
}
```

## Commands

> When calling the Execute() method, you do need to supply at least 1 parameter. Otherwise you end up with a recursive call, in this case it will lead to a stack overflow exception.

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

> Query results are mapped onto the Result Interface which also specifies the return type of the Generically defined class. When you would like to build a query you need to create a class that inherits from Query and specify the return type as a generic argument.

### Generic

```csharp
public class SelectAPerson : Query<Person>
{
    private readonly int _id;

    public SelectAPerson(int Id)
    {
        _id = Id;
    }
    public override void Execute()
    {
        Result = BuildSelect<Person>("where id = @Id", new { Id = _id });
    }
}
```

`BuildSelect` has a override for the where clause that can be appended to the end of the select statement. The second argument exists to specify the SQL query parameters.

### Non-Generic

Although this is possible it doesn't really make sense to do so because if you are writing a query, you would always expect something in return. You can in turn follow the same pattern as a Command and just call the `Execute()` override method if return values are not important.

## Writing Native Commands

You can also use the Dapper Doodle package to write native (SQL) commands if the command builders do not quite solve your problem.

### List Parameter Arguments

The DapperDoodle package supports the option for list parameters for Commands.

```csharp
public class InsertManyPeople : Command
{
    public List<Person> People = new List<Person>()
    {
        new Person()
        {
            Name = GetRandomString(),
            Surname = GetRandomString(),
            Email = GetRandomEmail()
        },
        new Person()
        {
            Name = GetRandomString(),
            Surname = GetRandomString(),
            Email = GetRandomEmail()
        }
    };
    
    public override void Execute()
    {
        Execute(@"INSERT INTO People ( name, surname, email )
                VALUES (@Name, @Surname, @Email)",
                People);
    }
}
```

## Using Dapper Directly

### Fetching the IDbConnection Instance

The dapper library is an extension library that sits on top of the `IDbConnection` interface. So we can use Dapper Doodle's dependency registrations to fetch our `IDbConnection` instance to write Dapper queries natively for more complex queries.

```csharp
public class TestBaseExecutor : Query<int>
{
    public override void Execute()
    {
        var connectionInstance = GetIDbConnection();
        var result = connectionInstance.QueryFirst("SELECT 1;");
        Result = result;
    }
}
```

More complex join statements to achieve deep linking (Eager Loading)

```csharp
public Person MapPersonAndAddress(int personId)
{
    using(IDbConnection cnn = GetIDbConnection()))
    {
        var param = new
        {
            Id = personId
        };
        // This SqlCommand returns a person with the address of that person.
        var sql = @"select u.*, a.* from `USERS` u 
        left join ADDRESSES a 
        on u.address_id = a.Id WHERE u.Id = @Id";
        
        // This is a dapper command that takes in two generic parameter objects and the last one will be the one that is mapped to, which in this case is the same model
        var people = cnn.Query<Person, Address, Person>(sql, (person, address) =>
            {
                person.Address = address;
                return person;
            },
            param
        );
        return people.FirstOrDefault();
    }
}
```

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
- MSSQL (Still needs to be added and tested)

# Feature List

- [x]  Command Builder
- [x]  Query Builder
- [x]  Auto-Insert Statements
- [x]  Auto-Update Statements
- [x]  Auto-Select Statements
- [x]  CQRS Interface helpers
- [x]  Support For MySql
- [ ]  Support For MSSQL
- [x]  Support For SqlLite
- [ ]  Support For PostgreSql

# Pull Requests

If you would like to contribute to this package you are welcome to do so. Just fork this repository, create a PR against it and add me as a reviewer.

## Testing Coverage

Testing coverage currently covers:

- QueryExecutor dependency injection
- CommandExecutor dependency Injection
- CommandBuilder queries for MySql, Sqlite, MSSQL.

### Coverage to be added

- Query Builder
- Variations on query builder
- Variations on command builder
- Testing Extension/Helper Classes
- Dependency registrations for MSSQL.
- Dependency registrations for MySql.
- Dependency registrations for SqlLite.

# Read Me

Please view the readme on the master branch [here](https://github.com/caybokotze/dapper-doodle).
