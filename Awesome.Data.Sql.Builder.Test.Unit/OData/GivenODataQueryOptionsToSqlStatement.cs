using System;
using System.Linq;
using Awesome.Data.Sql.Builder.OData;
using Microsoft.Data.OData;
using NUnit.Framework;

namespace Awesome.Data.Sql.Builder.Test.Unit.OData
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GivenODataQueryOptionsToSqlStatement
    {
        private ODataQueryOptionsToSqlStatement optionsToSql;

        [SetUp]
        public void SetUp()
        {
            this.optionsToSql = new ODataQueryOptionsToSqlStatement();
        }

        [Test]
        public void When_converting_with_select_properties_Then_selects_columns()
        {
            var options = ODataQueryOptionsHelper.Build<TestDTO>("$select=Id,Name");
            var result = this.optionsToSql.ToSelect(options).First();

            Assert.That(result.ColumnsList, Is.EquivalentTo(new[] { "Id", "Name" }));
        }

        [Test]
        public void When_converting_with_select_and_expand_properties_Then_selects_columns_and_maps_expands()
        {
            var options = ODataQueryOptionsHelper.Build<TestDTO>("$select=Id,Name,Contact/FirstName,Contact/BirthDate,Contact/Address/City&$expand=Contact,Contact/Address");
            var result = this.optionsToSql.ToSelect(options).First();

            Assert.That(result.ColumnsList, Is.EquivalentTo(new[] { "Id", "Name", "Contact/FirstName", "Contact/BirthDate", "Contact/Address/City" }));
        }

        [Test]
        public void When_converting_with_top_and_skip_Then_uses_limit_offset()
        {
            var options = ODataQueryOptionsHelper.Build<TestDTO>("$top=10&$skip=20");
            var result = this.optionsToSql.ToSelect(options).First();

            Assert.That(result.LimitClause, Is.EqualTo("10"));
            Assert.That(result.OffsetClause, Is.EqualTo("20"));
        }

        [Test]
        public void When_converting_invalid_columns_Then_throws()
        {
            var options = ODataQueryOptionsHelper.Build<TestDTO>("$select=SecretProperty");
            Assert.That(() => this.optionsToSql.ToSelect(options), Throws.Exception.InstanceOf<ODataException>());
        }

        [Test]
        public void When_converting_with_inline_count_Then_returns_two_selects()
        {
            var options = ODataQueryOptionsHelper.Build<TestDTO>("$select=Id,Name&$top=10&$skip=20&$inlinecount=allpages");
            var results = this.optionsToSql.ToSelect(options);

            Assert.That(results, Has.Count.EqualTo(2));
            var countSelect = results.Last();
            Assert.That(countSelect.ColumnsList, Is.EquivalentTo(new[] { "COUNT(*)" }));
            Assert.That(countSelect.OffsetClause, Is.Null);
            Assert.That(countSelect.LimitClause, Is.Null);
        }

        // ReSharper disable ClassNeverInstantiated.Local -- Only used for building OData query options.
        // ReSharper disable UnusedMember.Local
        private class TestDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }

            public decimal Amount { get; set; }

            public SubTestDTO Contact { get; set; }
        }

        private class SubTestDTO
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public DateTime BirthDate { get; set; }

            public SubSubTestDTO Address { get; set; }
        }

        private class SubSubTestDTO
        {
            public string StreetName { get; set; }

            public string City { get; set; }
        }
        //// ReSharper enable UnusedMember.Local
        // ReSharper enable ClassNeverInstantiated.Local
    }
}
