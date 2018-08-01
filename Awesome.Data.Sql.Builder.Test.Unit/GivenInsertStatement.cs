using Awesome.Data.Sql.Builder.Insert;
using FluentAssertions;
using Xunit;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    public class GivenInsertStatement
    {
        [Fact]
        public void When_inserting_rows_Then_builds_properly()
        {
            var statement = SqlStatements.Insert().Columns(new[] { "Name", "EmailAddress" })
                .Into("Users")
                .Rows(3);

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"INSERT INTO Users
    (
        Name,
        EmailAddress
    )
VALUES
    (
        @Name0,
        @EmailAddress0
    ),
    (
        @Name1,
        @EmailAddress1
    ),
    (
        @Name2,
        @EmailAddress2
    )");
        }

        [Fact]
        public void When_inserting_one_row_Then_builds_properly()
        {
            var statement = new InsertStatement(new[] { "Name", "EmailAddress" })
                .Into("Users");

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"INSERT INTO Users
    (
        Name,
        EmailAddress
    )
VALUES
    (
        @Name,
        @EmailAddress
    )");
        }

        [Fact]
        public void When_inserting_from_select_Then_builds_properly()
        {
            var select = SqlStatements.Select("Name", "EmailAddress")
                .From("Users")
                .InnerJoin("Teams", "Users.TeamID = Teams.ID")
                .Where("Teams.IsOld = FALSE");
            var statement = new InsertStatement(select)
                .Into("Users");

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"INSERT INTO Users
    (
        Name,
        EmailAddress
    )
SELECT
    Name, EmailAddress
FROM
    Users
    INNER JOIN Teams ON Users.TeamID = Teams.ID
WHERE
    Teams.IsOld = FALSE");
        }
    }
}
