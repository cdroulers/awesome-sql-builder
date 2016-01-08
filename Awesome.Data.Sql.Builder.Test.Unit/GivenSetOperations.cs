using Awesome.Data.Sql.Builder.Select;
using Awesome.Data.Sql.Builder.Test.Unit.Contraints;
using NUnit.Framework;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GivenSetOperations
    {
        [Test]
        public void When_building_union_all_Then_builds_properly()
        {
            var first = SqlStatements.Select("*").From("Users u");
            var second = SqlStatements.Select("*").From("Teams t");
            var union = new UnionOperation(first, second, true);

            Assert.That(
                union.ToSql(),
                SqlCompareConstraint.EqualTo(@"SELECT
    *
FROM
    Users u

UNION ALL

SELECT
    *
FROM
    Teams t"));
        }

        [Test]
        public void When_building_union_Then_builds_properly()
        {
            var first = SqlStatements.Select("*").From("Users u");
            var second = SqlStatements.Select("*").From("Teams t");
            var union = new UnionOperation(first, second);

            Assert.That(
                union.ToSql(),
                SqlCompareConstraint.EqualTo(@"SELECT
    *
FROM
    Users u

UNION

SELECT
    *
FROM
    Teams t"));
        }
    }
}
