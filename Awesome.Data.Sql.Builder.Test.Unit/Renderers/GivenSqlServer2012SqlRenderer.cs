using Awesome.Data.Sql.Builder.Renderers;
using Awesome.Data.Sql.Builder.Select;
using FluentAssertions;
using Xunit;

namespace Awesome.Data.Sql.Builder.Test.Unit.Renderers
{
    public class GivenSqlServer2012SqlRenderer
    {
        private readonly ISqlRenderer renderer;
        
        public GivenSqlServer2012SqlRenderer()
        {
            this.renderer = new SqlServer2012SqlRenderer();
        }

        [Fact]
        public void When_offsetting_and_limiting_Then_renders_differently()
        {
            var statement = new SelectStatement(new[] { "u.ID" })
                .From("Users u")
                .Limit(3)
                .Offset(6);

            var sql = this.renderer.RenderSelect(statement);

            sql.Should().BeEquivalentToIgnoringNewLines(@"SELECT" + "\n" + @"    u.ID
FROM
    Users u
OFFSET 6 ROWS
FETCH NEXT 3 ROWS ONLY");
        }
    }
}
