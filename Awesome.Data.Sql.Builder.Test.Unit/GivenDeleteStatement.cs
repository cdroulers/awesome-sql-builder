using Awesome.Data.Sql.Builder.Delete;
using Awesome.Data.Sql.Builder.Test.Unit.Contraints;
using NUnit.Framework;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    [TestFixture]
    public class GivenDeleteStatement
    {
        [Test]
        public void When_deleting_Then_builds_properly()
        {
            var statement = SqlStatements.Delete()
                .From("Users")
                .Where("u.IsCool = TRUE")
                .Where("u.Name LIKE @Query");

            var sql = statement.ToSql();

            Assert.That(
                sql,
                SqlCompareConstraint.EqualTo(@"DELETE Users
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query"));
        }

        [Test]
        public void When_deleting_from_two_tables_Then_builds_properly()
        {
            var statement = new DeleteStatement(tableToDelete: "u")
                .From("Users u")
                .InnerJoin("Teams t", "u.TeamID = t.ID")
                .Where("t.IsOld = TRUE");

            var sql = statement.ToSql();

            Assert.That(
                sql,
                SqlCompareConstraint.EqualTo(@"DELETE u
FROM
    Users u
    INNER JOIN Teams t ON u.TeamID = t.ID
WHERE
    t.IsOld = TRUE"));
        }
    }
}
