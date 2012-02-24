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
    public class GivenFromClause
    {
        [Test]
        public void When_converting_implicitely_Then_works()
        {
            FromClause clause = "Teams t";

            Assert.That(clause, Is.InstanceOf<TableClause>());
            Assert.That(clause.ToSql(), Is.EqualTo("Teams t"));
        }
    }
}
