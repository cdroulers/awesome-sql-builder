using Awesome.Data.Sql.Builder.Renderers;
using Awesome.Data.Sql.Builder.Select;
using Awesome.Data.Sql.Builder.Test.Unit.Contraints;
using NUnit.Framework;

namespace Awesome.Data.Sql.Builder.Test.Unit.Renderers
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GivenSqlServer2012SqlRenderer
    {
        private ISqlRenderer renderer;

        [SetUp]
        public void SetUp()
        {
            this.renderer = new SqlServer2012SqlRenderer();
        }

        [Test]
        public void When_offsetting_and_limiting_Then_renders_differently()
        {
            var statement = new SelectStatement(new[] { "u.ID" })
                .From("Users u")
                .Limit(3)
                .Offset(6);

            var sql = this.renderer.RenderSelect(statement);

            Assert.That(
                sql,
                SqlCompareConstraint.EqualTo(@"SELECT
    u.ID
FROM
    Users u
OFFSET 6 ROWS
FETCH NEXT 3 ROWS ONLY"));
        }
    }
}
