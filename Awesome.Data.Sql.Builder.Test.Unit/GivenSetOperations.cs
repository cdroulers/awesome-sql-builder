using Awesome.Data.Sql.Builder.Select;
using FluentAssertions;
using Xunit;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    public class GivenSetOperations
    {
        [Fact]
        public void When_building_union_all_Then_builds_properly()
        {
            var first = SqlStatements.Select("*").From("Users u");
            var second = SqlStatements.Select("*").From("Teams t");
            var union = new UnionOperation(first, second, true);

            union.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    *
FROM
    Users u

UNION ALL

SELECT
    *
FROM
    Teams t");
        }

        [Fact]
        public void When_building_union_Then_builds_properly()
        {
            var first = SqlStatements.Select("*").From("Users u");
            var second = SqlStatements.Select("*").From("Teams t");
            var union = new UnionOperation(first, second);

            union.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    *
FROM
    Users u

UNION

SELECT
    *
FROM
    Teams t");
        }
    }
}
