# Awesome.Data.Sql.Builder

A small library for building SQL queries in a better way than regular string concatenation.

Clauses can be added in any order.

Mostly for use directly by ADO.NET or micro-ORMs such as Dapper.

## Installation

`Install-Package Awesome.Data.Sql.Builder`

## Usage

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
    Is.EqualTo(@"SELECT
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

## Contributing

1. Fork it!
1. Create your feature branch: `git checkout -b my-new-feature`
1. Commit your changes: `git commit -am 'Add some feature'`
1. Make sure you add a unit test!
1. Push to the branch: `git push origin my-new-feature`
1. Submit a pull request :D

## History

### 1.0.0 

* First public version. Supports all basic SQL operations for PostgreSQL.

## Credits

Author: [cdroulers](https://github.com/cdroulers)

## License

LGPL: See [LICENSE](LICENSE)

## Roadmap

### 1.1.0

* Support for different SQL providers (SQL Server in particular) in a pluggable way.