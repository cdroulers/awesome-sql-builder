using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Awesome.Data.Sql.Builder.Renderers;
using Awesome.Data.Sql.Builder.Select;

namespace Awesome.Data.Sql.Builder.Insert
{
    /// <summary>
    /// Represents an SQL INSERT statement.
    /// </summary>
    public class InsertStatement : ICloneable, ISqlFragment
    {
        private readonly List<string> columnsList;

        /// <summary>
        /// Allows inserting multiple rows of data.
        /// </summary>
        public int? RowCount { get; set; }

        /// <summary>
        /// The table to insert into.
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Returns all columns
        /// </summary>
        public IReadOnlyCollection<string> ColumnsList
        {
            get { return new ReadOnlyCollection<string>(this.columnsList); }
        }

        /// <summary>
        /// Allows insert multiple rows from a SELECT statement
        /// </summary>
        public SelectStatement Select { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertStatement"/> class that will insert RAW data.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="table">The table to insert into.</param>
        /// <param name="rows">How many time to repeat the values row</param>
        public InsertStatement(IEnumerable<string> columns, string table = null, int? rows = null)
        {
            this.columnsList = new List<string>(columns);
            this.Table = table;
            this.RowCount = rows;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertStatement"/> class that will insert data from a SELECT.
        /// </summary>
        /// <param name="select">A SELECT statement which matches the columns to insert.</param>
        /// <param name="table">The table to insert into.</param>
        public InsertStatement(SelectStatement select, string table = null)
        {
            this.columnsList = new List<string>();
            this.Select = select;
            this.Table = table;
        }

        private InsertStatement(InsertStatement statement)
        {
            this.columnsList = statement.columnsList != null ? statement.columnsList.ToList() : null;
            this.RowCount = statement.RowCount;
            this.Select = statement.Select;
        }

        /// <summary>
        /// Only used in <see cref="SqlStatements"/> to start a fluent chain.
        /// </summary>
        internal InsertStatement()
        {
            this.columnsList = new List<string>();
        }

        /// <summary>
        /// This INSERT will get its data from the SELECT statement specified.
        /// </summary>
        /// <param name="selectStatement">A valid SELECT statement.</param>
        /// <returns>Itself for fluentness.</returns>
        public InsertStatement From(SelectStatement selectStatement)
        {
            this.Select = selectStatement;
            return this;
        }

        /// <summary>
        /// Adds columns to SELECT
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public InsertStatement Columns(params string[] columns)
        {
            return this.Columns(false, columns);
        }

        /// <summary>
        /// Sets which table to insert into.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>Itself for fluentness</returns>
        public InsertStatement Into(string table)
        {
            this.Table = table;
            return this;
        }

        /// <summary>
        /// Sets the number of rows to insert
        /// </summary>
        /// <param name="rows">Number of rows to repeat</param>
        /// <returns>Itself for fluentness</returns>
        public InsertStatement Rows(int rows)
        {
            this.RowCount = rows;
            return this;
        }

        /// <summary>
        /// Adds columns to SELECT. Clears the current one if specified.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="clearCurrent">if set to <c>true</c> [clear current].</param>
        /// <returns></returns>
        public InsertStatement Columns(bool clearCurrent, params string[] columns)
        {
            if (clearCurrent)
            {
                this.columnsList.Clear();
            }

            this.columnsList.AddRange(columns);
            return this;
        }

        /// <summary>
        /// Returns the statement as an SQL string.
        /// </summary>
        public void BuildSql(StringBuilder builder, ISqlRenderer renderer)
        {
            builder.Append(renderer.RenderInsert(this));
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public InsertStatement Clone()
        {
            return new InsertStatement(this);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Adds the SQL for the current object to the builder
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void BuildSql(StringBuilder builder)
        {
            this.BuildSql(builder, new DefaultSqlRenderer());
        }
    }
}
