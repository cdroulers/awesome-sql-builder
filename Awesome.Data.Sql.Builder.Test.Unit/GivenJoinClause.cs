using Awesome.Data.Sql.Builder.Select;
using Awesome.Data.Sql.Builder.Test.Unit.Contraints;
using NUnit.Framework;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GivenJoinClause
    {
        [Test]
        public void When_building_Then_builds_properly()
        {
            var join = new OuterJoin(new TableClause("Lol l"), new TableClause("Wat w"), "l.WatID = w.ID");

            Assert.That(
                join.ToSql(), 
                SqlCompareConstraint.EqualTo(@"Lol l
    OUTER JOIN Wat w ON l.WatID = w.ID"));
        }

        [Test]
        public void When_building_nested_Then_builds_properly()
        {
            var join = new OuterJoin(new TableClause("Lol l"), new TableClause("Wat w"), "l.WatID = w.ID");

            var innerJoin = new InnerJoin(join, new TableClause("Derp d"), "l.DerpID = d.ID");

            Assert.That(
                innerJoin.ToSql(), 
                SqlCompareConstraint.EqualTo(@"Lol l
    OUTER JOIN Wat w ON l.WatID = w.ID
    INNER JOIN Derp d ON l.DerpID = d.ID"));
        }

        [Test]
        public void When_building_triple_nested_Then_builds_properly()
        {
            var join = new OuterJoin(new TableClause("Lol l"), new TableClause("Wat w"), "l.WatID = w.ID");

            var innerJoin = new InnerJoin(join, new TableClause("Derp d"), "l.DerpID = d.ID");

            var fullJoin = new FullJoin(innerJoin, new TableClause("Herp h"), "l.HerpID = h.ID");

            Assert.That(
                fullJoin.ToSql(),
                SqlCompareConstraint.EqualTo(@"Lol l
    OUTER JOIN Wat w ON l.WatID = w.ID
    INNER JOIN Derp d ON l.DerpID = d.ID
    FULL JOIN Herp h ON l.HerpID = h.ID"));
        }
    }
}
