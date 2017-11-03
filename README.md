# Awesome.Data.Sql.Builder

Travis CI: [![Build Status](https://travis-ci.org/cdroulers/awesome-sql-builder.svg)](https://travis-ci.org/cdroulers/awesome-sql-builder)

A small library for building SQL queries in a better way than regular string concatenation.

Clauses can be added in any order.

Mostly for use directly by ADO.NET or micro-ORMs such as Dapper.

## Awesome.Data.Sql.Builder.OData

A small helper project that allows transforming `ODataQueryOptions<T>` directly into
a `SelectStatement`.

## Installation

[NuGet Awesome.Data.Sql.Builder](https://www.nuget.org/packages/Awesome.Data.Sql.Builder/)

`Install-Package Awesome.Data.Sql.Builder`

[NuGet Awesome.Data.Sql.Builder.OData](https://www.nuget.org/packages/Awesome.Data.Sql.Builder.OData/)

`Install-Package Awesome.Data.Sql.Builder.OData`

## Awesome.Data.Sql.Builder Usage

### SELECT Statement

Here is a sample unit test from the project.

```csharp
var statement = SqlStatements.Select(new[] { "u.ID", "u.Name", "u.EmailAddress" })
    .From("Users u")
    .Where("u.IsCool = TRUE")
    .Where("u.Name LIKE @Query")
    .OrderBy("u.Name", false)
    .Limit(3).Offset(6);

var sql = statement.ToSql();

Assert.That(
    sql,
    SqlCompareConstraint.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query
ORDER BY
    u.Name DESC
LIMIT 3 OFFSET 6"));
```

### UPDATE Statement

Sample unit test!

```csharp
var statement = new UpdateStatement(new[] { "ID", "Name", "EmailAddress" })
    .From("Users")
    .Where("u.IsCool = TRUE")
    .Where("u.Name LIKE @Query");

var sql = statement.ToSql();

Assert.That(
    sql,
    SqlCompareConstraint.EqualTo(@"UPDATE Users
SET
    ID = @ID,
    Name = @Name,
    EmailAddress = @EmailAddress
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query"));
```


### Different provider

```csharp
var statement = new SelectStatement(new[] { "u.ID" })
    .From("Users u")
    .Limit(3)
    .Offset(6);

var sql = new SqlServer2012SqlRenderer().RenderSelect(statement);

Assert.That(
    sql,
    SqlCompareConstraint.EqualTo(@"SELECT
    u.ID
FROM
    Users u
OFFSET 6 ROWS
FETCH NEXT 3 ROWS ONLY"));
```

## Awesome.Data.Sql.Builder.OData Usage

```csharp
var options = ODataQueryOptionsHelper.Build<TestDTO>(
    "$select=Id,Name,Contact/FirstName,Contact/BirthDate,Contact/Address/City&" + 
    "$expand=Contact,Contact/Address&" +
    "$top=10&" +
    "$skip=20");
var result = new ODataQueryOptionsToSqlStatement().ToSelect(options).First();

Assert.That(
    result.ColumnsList,
    Is.EquivalentTo(new[]
    {
        "Id",
        "Name",
        "Contact/FirstName",
        "Contact/BirthDate",
        "Contact/Address/City"
    }));
Assert.That(result.LimitClause, Is.EqualTo("10"));
Assert.That(result.OffsetClause, Is.EqualTo("20"));
```

Only supports `$select`, `$top`, `$skip` and `$inlinecount=allpages` for now!

## Contributing

1. Fork it!
1. Create your feature branch: `git checkout -b my-new-feature`
1. Commit your changes: `git commit -am 'Add some feature'`
1. Make sure you add a unit test!
1. Push to the branch: `git push origin my-new-feature`
1. Submit a pull request :D

## Credits

Author: [cdroulers](https://github.com/cdroulers)

## License

LGPL: See [LICENSE](LICENSE)

## Awesome.Data.Sql.Builder History

### 1.1.0 (2016-01-18)

* Support for different SQL providers (SQL Server in particular) in a pluggable way.

### 1.0.0 (2012-11-16)

* First public version. Supports all basic SQL operations for PostgreSQL.

## Roadmap

### 1.2.0

* ???

## Awesome.Data.Sql.Builder.OData History

### 1.0.0 (2016-01-23)

* First public version. Supports `$select`, `$top`, `$skip` and `$inlinecount=allpages`.

## Roadmap

### 1.1.0

* Support basic `$filter` operations