# DbMigrator

[![Build status](https://ci.appveyor.com/api/projects/status/ty4dr4v2j9qu9w1s?svg=true)](https://ci.appveyor.com/project/Altafard/dbmigrator) [![NuGet version](https://badge.fury.io/nu/AltaDigital.DbMigrator.svg)](https://badge.fury.io/nu/AltaDigital.DbMigrator)

Simple migration for databases on .NET

## Usage

```c#
public void ConfigureServices(IServiceCollection services)
{
    // ...
    
    services.AddDbMigrator(cfg => cfg.Use<NpgsqlMigrationContext>( ... ));
}
```

Usage `NpgsqlMigrationContext` allow you migrate postgresql database. `NpgsqlMigrationContext` is available in repo [DbMigrator.Npgsql](https://github.com/Altafard/DbMigrator.Npgsql). If you want use other provider, you can implement own migration context by inherit IMigrationContext.

```c#
public void Configure(IApplicationBuilder app)
{
    // ...
    
    app.UseDbMigrator(async migrator => await migrator.Migrate());
    // or
    app.UseDbMigrator(async migrator => await migrator.Downgrade(1));
}
```

Now create your migration which inherits from `MigrationBase` abstract class.

```c#
public class CreateTableMigration : MigrationBase
{
    public override long Key => 1;

    public override void Up(IMigrationAction action)
    {
        action.ExecuteSql("CREATE TABLE test(id int, name text);");
    }

    public override void Down(IMigrationAction action)
    {
        action.ExecuteSql("DROP TABLE test;");
    }
}
```

All implementations of `IMigration` will automatically added to service collection, if they are placed in current assembly. If not, then you can pointed which assembly to use, as shown below

```c#
services.AddDbMigrator(cfg => cfg.Use<NpgsqlMigrationContext>( ... ), typeof(CreateTableMigration).Assembly);
```

## Feedback

All contributions are welcome. I will glad to discuss all suggestions and troubles.
