using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data.Sql.Builder.Select;
using System.Data.Sql.Builder;

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

            Assert.That(join.ToSql(), Is.EqualTo(@"Lol l
    OUTER JOIN Wat w ON l.WatID = w.ID"));
        }

        [Test]
        public void When_building_nested_Then_builds_properly()
        {
            var join = new OuterJoin(new TableClause("Lol l"), new TableClause("Wat w"), "l.WatID = w.ID");

            var innerJoin = new InnerJoin(join, new TableClause("Derp d"), "l.DerpID = d.ID");

            Assert.That(innerJoin.ToSql(), Is.EqualTo(@"Lol l
    OUTER JOIN Wat w ON l.WatID = w.ID
    INNER JOIN Derp d ON l.DerpID = d.ID"));
        }

        [Test]
        public void When_building_triple_nested_Then_builds_properly()
        {
            var join = new OuterJoin(new TableClause("Lol l"), new TableClause("Wat w"), "l.WatID = w.ID");

            var innerJoin = new InnerJoin(join, new TableClause("Derp d"), "l.DerpID = d.ID");

            var fullJoin = new FullJoin(innerJoin, new TableClause("Herp h"), "l.HerpID = h.ID");

            Assert.That(fullJoin.ToSql(), Is.EqualTo(@"Lol l
    OUTER JOIN Wat w ON l.WatID = w.ID
    INNER JOIN Derp d ON l.DerpID = d.ID
    FULL JOIN Herp h ON l.HerpID = h.ID"));
        }
    }
}
