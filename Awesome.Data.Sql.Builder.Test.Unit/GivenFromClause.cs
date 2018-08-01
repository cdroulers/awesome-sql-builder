using FluentAssertions;
using Xunit;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    public class GivenFromClause
    {
        [Fact]
        public void When_converting_implicitely_Then_works()
        {
            FromClause clause = "Teams t";

            clause.Should().BeOfType<TableClause>();
            clause.ToSql().Should().Be("Teams t");
        }

        [Fact]
        public void When_adding_alias_Then_is_in_output()
        {
            FromClause clause = "Teams";
            clause.As("t");

            clause.Should().BeOfType<TableClause>();
            clause.ToSql().Should().Be("Teams t");
        }
    }
}
