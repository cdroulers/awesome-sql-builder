using Awesome.Data.Sql.Builder.Select;
using FluentAssertions;
using Xunit;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    public class GivenJoinClause
    {
        [Fact]
        public void When_building_Then_builds_properly()
        {
            var join = new OuterJoin(new TableClause("Lol l"), new TableClause("Wat w"), "l.WatID = w.ID");

            join.ToSql().Should().BeEquivalentToIgnoringNewLines(@"Lol l
    OUTER JOIN Wat w ON l.WatID = w.ID");
        }

        [Fact]
        public void When_building_nested_Then_builds_properly()
        {
            var join = new OuterJoin(new TableClause("Lol l"), new TableClause("Wat w"), "l.WatID = w.ID");

            var innerJoin = new InnerJoin(join, new TableClause("Derp d"), "l.DerpID = d.ID");

            innerJoin.ToSql().Should().BeEquivalentToIgnoringNewLines(@"Lol l
    OUTER JOIN Wat w ON l.WatID = w.ID
    INNER JOIN Derp d ON l.DerpID = d.ID");
        }

        [Fact]
        public void When_building_triple_nested_Then_builds_properly()
        {
            var join = new OuterJoin(new TableClause("Lol l"), new TableClause("Wat w"), "l.WatID = w.ID");

            var innerJoin = new InnerJoin(join, new TableClause("Derp d"), "l.DerpID = d.ID");

            var fullJoin = new FullJoin(innerJoin, new TableClause("Herp h"), "l.HerpID = h.ID");

            fullJoin.ToSql().Should().BeEquivalentToIgnoringNewLines(@"Lol l
    OUTER JOIN Wat w ON l.WatID = w.ID
    INNER JOIN Derp d ON l.DerpID = d.ID
    FULL JOIN Herp h ON l.HerpID = h.ID");
        }
    }
}
