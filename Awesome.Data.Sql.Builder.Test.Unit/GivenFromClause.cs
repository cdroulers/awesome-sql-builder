using Awesome.Data.Sql.Builder.Test.Unit.Contraints;
using NUnit.Framework;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GivenFromClause
    {
        [Test]
        public void When_converting_implicitely_Then_works()
        {
            FromClause clause = "Teams t";

            Assert.That(clause, Is.InstanceOf<TableClause>());
            Assert.That(clause.ToSql(), SqlCompareConstraint.EqualTo("Teams t"));
        }

        [Test]
        public void When_adding_alias_Then_is_in_output()
        {
            FromClause clause = "Teams";
            clause.As("t");

            Assert.That(clause, Is.InstanceOf<TableClause>());
            Assert.That(clause.ToSql(), SqlCompareConstraint.EqualTo("Teams t"));
        }
    }
}
