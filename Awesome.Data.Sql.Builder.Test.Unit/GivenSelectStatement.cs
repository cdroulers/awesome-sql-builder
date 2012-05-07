using System;
using System.Collections.Generic;
using System.Data.Sql.Builder;
using System.Data.Sql.Builder.Select;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GivenSelectStatement
    {
        [Test]
        public void When_selecting_Then_builds_properly()
        {
            var statement = new SelectStatement(new string[] { "u.ID", "u.Name", "u.EmailAddress" })
                .From("Users u")
                .Where("u.IsCool = TRUE")
                .Where("u.Name LIKE @Query")
                .OrderBy("u.Name", false);

            var sql = statement.ToSql();

            Assert.That(sql, Is.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query
ORDER BY
    u.Name DESC"));
        }

        [Test]
        public void When_selecting_with_limit_and_offset_Then_builds_properly()
        {
            var statement = new SelectStatement(new string[] { "u.ID", "u.Name", "u.EmailAddress" })
                .From("Users u")
                .Where("u.IsCool = TRUE")
                .Where("u.Name LIKE @Query")
                .OrderBy("u.Name", false)
                .Limit(3).Offset(6);

            var sql = statement.ToSql();

            Assert.That(sql, Is.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query
ORDER BY
    u.Name DESC
LIMIT 3 OFFSET 6"));
        }

        [Test]
        public void When_selecting_with_or_Then_builds_properly()
        {
            var statement = new SelectStatement(new string[] { "u.ID", "u.Name", "u.EmailAddress" })
                .From("Users u")
                .Where("u.IsCool = TRUE", or: true)
                .Where("u.Name LIKE @Query")
                .OrderBy("u.Name", false);

            var sql = statement.ToSql();

            Assert.That(sql, Is.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE OR
    u.Name LIKE @Query
ORDER BY
    u.Name DESC"));
        }

        [Test]
        public void When_selecting_not_in_order_Then_builds_properly()
        {
            var statement = new SelectStatement(new string[] { "u.ID", "u.Name", "u.EmailAddress" })
                .Where("u.IsCool = TRUE")
                .From("Users u")
                .Where("u.Name LIKE @Query")
                .OrderBy("u.Name", false);

            var sql = statement.ToSql();

            Assert.That(sql, Is.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query
ORDER BY
    u.Name DESC"));
        }

        [Test]
        public void When_selecting_not_in_order_with_multiple_order_by_Then_builds_properly()
        {
            var statement = new SelectStatement(new string[] { "u.ID", "u.Name", "u.EmailAddress" })
                .Where("u.IsCool = TRUE")
                .OrderBy("u.EmailAddress")
                .From("Users u")
                .Where("u.Name LIKE @Query")
                .OrderBy("u.Name", false);

            var sql = statement.ToSql();

            Assert.That(sql, Is.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND
    u.Name LIKE @Query
ORDER BY
    u.EmailAddress ASC, u.Name DESC"));
        }

        [Test]
        public void When_selecting_with_multiple_values_Then_builds_properly()
        {
            var statement = SqlStatements.Select("u.ID, u.Name, u.EmailAddress")
                .Where("u.IsCool = TRUE AND u.Name LIKE @Query")
                .OrderBy("u.EmailAddress")
                .From("Users u")
                .OrderBy("u.Name", false);

            var sql = statement.ToSql();

            Assert.That(sql, Is.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u
WHERE
    u.IsCool = TRUE AND u.Name LIKE @Query
ORDER BY
    u.EmailAddress ASC, u.Name DESC"));
        }

        [Test]
        public void When_cloning_Then_copies_stuff()
        {
            var statement = SqlStatements.Select("u.ID, u.Name, u.EmailAddress")
                .From("Users u");

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u"));

            var clone = statement.Clone();
            clone.From("Teams t")
                .Columns("u.IsCool")
                .Where("u.Name = @Query")
                .OrderBy("u.Name", false);

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u"));
            Assert.That(clone.ToSql(), Is.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress, u.IsCool
FROM
    Users u,
    Teams t
WHERE
    u.Name = @Query
ORDER BY
    u.Name DESC"));
        }

        [Test]
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

            Assert.That(statement.ToSql(), Is.EqualTo(resultStatement));

            var clone = statement.Clone();

            Assert.That(statement.ToSql(), Is.EqualTo(resultStatement));
            Assert.That(clone.ToSql(), Is.EqualTo(resultStatement));
        }

        [Test]
        public void When_clearing_columns_Then_empties_list()
        {
            var statement = SqlStatements.Select("u.ID, u.Name, u.EmailAddress")
                .From("Users u");

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID, u.Name, u.EmailAddress
FROM
    Users u"));

            statement.Columns(true, "u.ID");
            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID
FROM
    Users u"));
        }

        [Test]
        public void When_selecting_with_outer_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .OuterJoin(new TableClause("Teams t"), "u.TeamID = t.ID");

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID, t.ID
FROM
    Users u
    OUTER JOIN Teams t ON u.TeamID = t.ID"));
        }

        [Test]
        public void When_selecting_with_inner_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .InnerJoin(new TableClause("Teams t"), "u.TeamID = t.ID");

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID, t.ID
FROM
    Users u
    INNER JOIN Teams t ON u.TeamID = t.ID"));
        }

        [Test]
        public void When_selecting_with_left_outer_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .LeftOuterJoin(new TableClause("Teams t"), "u.TeamID = t.ID");

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID, t.ID
FROM
    Users u
    LEFT OUTER JOIN Teams t ON u.TeamID = t.ID"));
        }

        [Test]
        public void When_selecting_with_right_outer_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .RightOuterJoin(new TableClause("Teams t"), "u.TeamID = t.ID");

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID, t.ID
FROM
    Users u
    RIGHT OUTER JOIN Teams t ON u.TeamID = t.ID"));
        }

        [Test]
        public void When_selecting_with_full_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .FullJoin(new TableClause("Teams t"), "u.TeamID = t.ID");

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID, t.ID
FROM
    Users u
    FULL JOIN Teams t ON u.TeamID = t.ID"));
        }

        [Test]
        public void When_selecting_with_triple_join_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, t.ID")
                .From("Users u")
                .FullJoin(new TableClause("Teams t"), "u.TeamID = t.ID")
                .OuterJoin(new TableClause("Settings s"), "u.SettingID = s.ID")
                .LeftOuterJoin(new TableClause("Parameters p"), "u.ParameterID = p.ID");

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID, t.ID
FROM
    Users u
    FULL JOIN Teams t ON u.TeamID = t.ID
    OUTER JOIN Settings s ON u.SettingID = s.ID
    LEFT OUTER JOIN Parameters p ON u.ParameterID = p.ID"));
        }

        [Test]
        public void When_selecting_with_group_by_Then_works()
        {
            var statement = SqlStatements.Select("u.ID, COUNT(*)")
                .From("Users u")
                .GroupBy("u.ID");

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    u.ID, COUNT(*)
FROM
    Users u
GROUP BY
    u.ID"));
        }

        [Test]
        public void When_selecting_with_no_table_by_Then_doesnt_output_from_clause()
        {
            var statement = SqlStatements.Select("(SELECT COUNT(*) FROM Users) AS UserCount");

            Assert.That(statement.ToSql(), Is.EqualTo(@"SELECT
    (SELECT COUNT(*) FROM Users) AS UserCount"));
        }
    }
}
