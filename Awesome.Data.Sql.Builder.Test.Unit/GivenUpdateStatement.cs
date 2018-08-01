using Awesome.Data.Sql.Builder.Update;
using FluentAssertions;
using Xunit;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    public class GivenUpdateStatement
    {
        [Fact]
        public void When_updating_Then_builds_properly()
        {
            var statement = SqlStatements.Update("ID", "Name", "EmailAddress")
                .From("Users")
                .Where("u.IsCool = TRUE")
                .Where("u.Name LIKE @Query");

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"UPDATE Users
SET
    ID = @ID,
    Name = @Name,
    EmailAddress = @EmailAddress
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query");
        }

        [Fact]
        public void When_updating_from_two_tables_Then_builds_properly()
        {
            var statement = new UpdateStatement(new[] { "Name" }, tableToUpdate: "u")
                .From("Users u")
                .InnerJoin("Teams t", "u.TeamID = t.ID")
                .Where("t.IsOld = TRUE");

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"UPDATE u
SET
    Name = @Name
FROM
    Users u
    INNER JOIN Teams t ON u.TeamID = t.ID
WHERE
    t.IsOld = TRUE");
        }
    }
}
