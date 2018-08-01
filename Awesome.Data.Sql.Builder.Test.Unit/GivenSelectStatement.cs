using Awesome.Data.Sql.Builder.Select;
using FluentAssertions;
using Xunit;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    public class GivenSelectStatement
    {
        [Fact]
        public void When_selecting_Then_builds_properly()
        {
            var statement = new SelectStatement(new[] { "u.ID", "u.Name", "u.EmailAddress" })
                .From("Users u")
                .Where("u.IsCool = TRUE")
                .Where("u.Name LIKE @Query")
                .OrderBy("u.Name", false);

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query
ORDER BY
    u.Name DESC");
        }

        [Fact]
        public void When_selecting_with_limit_and_offset_Then_builds_properly()
        {
            var statement = new SelectStatement(new[] { "u.ID", "u.Name", "u.EmailAddress" })
                .From("Users u")
                .Where("u.IsCool = TRUE")
                .Where("u.Name LIKE @Query")
                .OrderBy("u.Name", false)
                .Limit(3).Offset(6);

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query
ORDER BY
    u.Name DESC
LIMIT 3 OFFSET 6");
        }

        [Fact]
        public void When_selecting_with_or_Then_builds_properly()
        {
            var statement = new SelectStatement(new[] { "u.ID", "u.Name", "u.EmailAddress" })
                .From("Users u")
                .Where("u.IsCool = TRUE", true)
                .Where("u.Name LIKE @Query")
                .OrderBy("u.Name", false);

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE OR
    u.Name LIKE @Query
ORDER BY
    u.Name DESC");
        }

        [Fact]
        public void When_selecting_not_in_order_Then_builds_properly()
        {
            var statement = new SelectStatement(new[] { "u.ID", "u.Name", "u.EmailAddress" })
                .Where("u.IsCool = TRUE")
                .From("Users u")
                .Where("u.Name LIKE @Query")
                .OrderBy("u.Name", false);

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query
ORDER BY
    u.Name DESC");
        }

        [Fact]
        public void When_selecting_not_in_order_with_multiple_order_by_Then_builds_properly()
        {
            var statement = new SelectStatement(new[] { "u.ID", "u.Name", "u.EmailAddress" })
                .Where("u.IsCool = TRUE")
                .OrderBy("u.EmailAddress")
                .From("Users u")
                .Where("u.Name LIKE @Query")
                .OrderBy("u.Name", false);

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query
ORDER BY
    u.EmailAddress ASC, u.Name DESC");
        }

        [Fact]
        public void When_selecting_with_multiple_values_Then_builds_properly()
        {
            var statement = SqlStatements.Select("u.ID, u.Name, u.EmailAddress")
                .Where("u.IsCool = TRUE AND u.Name LIKE @Query")
                .OrderBy("u.EmailAddress")
                .From("Users u")
                .OrderBy("u.Name", false);

            var sql = statement.ToSql();

            sql.Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND u.Name LIKE @Query
ORDER BY
    u.EmailAddress ASC, u.Name DESC");
        }

        [Fact]
        public void When_cloning_Then_copies_stuff()
        {
            var statement = SqlStatements.Select("u.ID, u.Name, u.EmailAddress")
                .From("Users u");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u");

            var clone = statement.Clone();
            clone.From("Teams t")
                .Columns("u.IsCool")
                .Where("u.Name = @Query")
                .OrderBy("u.Name", false);

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u");
            clone.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, u.Name, u.EmailAddress, u.IsCool
FROM
    Users u,
    Teams t
WHERE
    u.Name = @Query
ORDER BY
    u.Name DESC");
        }

        [Fact]
        public void When_cloning_with_all_properties_Then_copies_stuff()
        {
            var statement = SqlStatements.Select("u.ID, u.Name, u.EmailAddress")
                .From("Users u")
                .Where("u.IsCool IS NULL")
                .GroupBy("u.ID")
                .OrderBy("u.Name")
                .Limit(1).Offset(2);

            const string resultStatement = @"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool IS NULL
GROUP BY
    u.ID
ORDER BY
    u.Name ASC
LIMIT 1 OFFSET 2";

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(resultStatement);

            var clone = statement.Clone();

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(resultStatement);
            clone.ToSql().Should().BeEquivalentToIgnoringNewLines(resultStatement);
        }

        [Fact]
        public void When_getting_count_statement_Then_removes_unnecessary_stuff()
        {
            var statement = SqlStatements.Select("u.ID, u.Name, u.EmailAddress")
                .From("Users u")
                .Where("u.IsCool IS NULL")
                .GroupBy("u.ID")
                .OrderBy("u.Name")
                .Limit(1).Offset(2);

            var count = statement.ToCount();

            count.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    COUNT(*)
FROM
    Users u
WHERE
    u.IsCool IS NULL
GROUP BY
    u.ID");
        }

        [Fact]
        public void When_clearing_columns_Then_empties_list()
        {
            var statement = SqlStatements.Select("u.ID, u.Name, u.EmailAddress")
                .From("Users u");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u");

            statement.Columns(true, "u.ID");
            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID
FROM
    Users u");
        }

        [Fact]
        public void When_selecting_with_outer_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .OuterJoin(new TableClause("Teams t"), "u.TeamID = t.ID");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, t.ID
FROM
    Users u
    OUTER JOIN Teams t ON u.TeamID = t.ID");
        }

        [Fact]
        public void When_selecting_with_inner_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .InnerJoin(new TableClause("Teams t"), "u.TeamID = t.ID");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, t.ID
FROM
    Users u
    INNER JOIN Teams t ON u.TeamID = t.ID");
        }

        [Fact]
        public void When_selecting_with_left_outer_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .LeftOuterJoin(new TableClause("Teams t"), "u.TeamID = t.ID");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, t.ID
FROM
    Users u
    LEFT OUTER JOIN Teams t ON u.TeamID = t.ID");
        }

        [Fact]
        public void When_selecting_with_right_outer_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .RightOuterJoin(new TableClause("Teams t"), "u.TeamID = t.ID");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, t.ID
FROM
    Users u
    RIGHT OUTER JOIN Teams t ON u.TeamID = t.ID");
        }

        [Fact]
        public void When_selecting_with_full_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .FullJoin(new TableClause("Teams t"), "u.TeamID = t.ID");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, t.ID
FROM
    Users u
    FULL JOIN Teams t ON u.TeamID = t.ID");
        }

        [Fact]
        public void When_selecting_with_triple_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .FullJoin(new TableClause("Teams t"), "u.TeamID = t.ID")
                .OuterJoin(new TableClause("Settings s"), "u.SettingID = s.ID")
                .LeftOuterJoin(new TableClause("Parameters p"), "u.ParameterID = p.ID");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, t.ID
FROM
    Users u
    FULL JOIN Teams t ON u.TeamID = t.ID
    OUTER JOIN Settings s ON u.SettingID = s.ID
    LEFT OUTER JOIN Parameters p ON u.ParameterID = p.ID");
        }

        [Fact]
        public void When_selecting_with_group_by_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, COUNT(*)")
                .From("Users u")
                .GroupBy("u.ID");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    u.ID, COUNT(*)
FROM
    Users u
GROUP BY
    u.ID");
        }

        [Fact]
        public void When_selecting_with_no_table_by_Then_doesnt_output_from_clause()
        {
            var statement = SqlStatements.Select("(SELECT COUNT(*) FROM Users) AS UserCount");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    (SELECT COUNT(*) FROM Users) AS UserCount");
        }

        [Fact]
        public void When_selecting_from_other_select_Then_outputs_properly()
        {
            var temp = SqlStatements.Select("u.ID").From("Users u").As("Sub");
            var statement = SqlStatements.Select("*").From(temp);
            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    *
FROM
    (
SELECT
    u.ID
FROM
    Users u
    ) Sub");
        }

        [Fact]
        public void When_selecting_with_union_all_Then_outputs_properly()
        {
            var firstUnion = SqlStatements.Select("u.ID").From("Users u");
            var secondUnion = SqlStatements.Select("t.ID").From("Teams t");
            var thirdUnion = SqlStatements.Select("w.ID").From("Wot w");

            var statement = SqlStatements.Select("*")
                .From(firstUnion.Union(secondUnion, all: true).Union(thirdUnion).As("Sub"))
                .Where("ID > 3");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    *
FROM
    (
SELECT
    u.ID
FROM
    Users u

UNION ALL

SELECT
    t.ID
FROM
    Teams t

UNION

SELECT
    w.ID
FROM
    Wot w
    ) Sub
WHERE
    ID > 3");
        }

        [Fact]
        public void When_selecting_with_intersect_and_except_Then_outputs_properly()
        {
            var first = SqlStatements.Select("u.ID").From("Users u");
            var second = SqlStatements.Select("t.ID").From("Teams t");
            var third = SqlStatements.Select("w.ID").From("Wot w");

            var statement = SqlStatements.Select("*")
                .From(first.Intersect(second).Except(third, all: true).As("Sub"))
                .Where("ID > 3");

            statement.ToSql().Should().BeEquivalentToIgnoringNewLines(@"SELECT
    *
FROM
    (
SELECT
    u.ID
FROM
    Users u

INTERSECT

SELECT
    t.ID
FROM
    Teams t

EXCEPT ALL

SELECT
    w.ID
FROM
    Wot w
    ) Sub
WHERE
    ID > 3");
        }
    }
}
