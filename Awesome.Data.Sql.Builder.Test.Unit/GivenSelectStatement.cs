using System;
using System.Collections.Generic;
using System.Data.Sql.Builder;
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
    }
}
