using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Awesome.Data.Sql.Builder.Renderers;

namespace Awesome.Data.Sql.Builder.Select
{
    /// <summary>
    ///     Represents an SQL statement
    /// </summary>
    public class SelectStatement : SqlStatement<SelectStatement>, ISetQuery
    {
        private readonly List<string> columnsList;
        private readonly List<GroupByClause> groupByClauses;
        private readonly List<OrderByClause> orderByClauses;
        private string limitClause;
        private string offsetClause;

        /// <summary>
        /// Returns
        /// </summary>
        public ReadOnlyCollection<string> ColumnsList
        {
            get { return new ReadOnlyCollection<string>(this.columnsList); }
        }

        /// <summary>
        /// Returns a list of group by clauses.
        /// </summary>
        public ReadOnlyCollection<GroupByClause> GroupByClauses
        {
            get { return new ReadOnlyCollection<GroupByClause>(this.groupByClauses); }
        }

        /// <summary>
        /// Returns a list of order by clauses.
        /// </summary>
        public ReadOnlyCollection<OrderByClause> OrderByClauses
        {
            get { return new ReadOnlyCollection<OrderByClause>(this.orderByClauses); }
        }

        /// <summary>
        /// Gets the limit clause
        /// </summary>
        public string LimitClause
        {
            get { return this.limitClause; }
        }

        /// <summary>
        /// Gets the offset clause
        /// </summary>
        public string OffsetClause
        {
            get { return this.offsetClause; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectStatement"/> class.
        /// </summary>
        /// <param name="columns">The columns.</param>
        public SelectStatement(IEnumerable<string> columns)
        {
            this.columnsList = new List<string>(columns);
            this.groupByClauses = new List<GroupByClause>();
            this.orderByClauses = new List<OrderByClause>();
        }

        private SelectStatement(SqlStatement<SelectStatement> statement, List<string> columns, List<GroupByClause> groupByClauses, List<OrderByClause> orderByClauses, string limitClause, string offsetClause)
            : base(statement)
        {
            this.columnsList = columns;
            this.groupByClauses = groupByClauses;
            this.orderByClauses = orderByClauses;
            this.limitClause = limitClause;
            this.offsetClause = offsetClause;
        }

        /// <summary>
        /// Adds columns to SELECT
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public SelectStatement Columns(params string[] columns)
        {
            return this.Columns(false, columns);
        }

        /// <summary>
        /// Adds columns to SELECT. Clears the current one if specified.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="clearCurrent">if set to <c>true</c> [clear current].</param>
        /// <returns></returns>
        public SelectStatement Columns(bool clearCurrent, params string[] columns)
        {
            if (clearCurrent)
            {
                this.columnsList.Clear();
            }

            this.columnsList.AddRange(columns);
            return this;
        }

        /// <summary>
        ///     Adds a column to the GROUP BY clause.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="clearCurrent">if set to <c>true</c> [clearCurrent]</param>
        /// <returns></returns>
        public SelectStatement GroupBy(GroupByClause column, bool clearCurrent = false)
        {
            if (clearCurrent)
            {
                this.groupByClauses.Clear();
            }

            this.groupByClauses.Add(column);
            return this;
        }

        /// <summary>
        /// Clears the GROUP BY clauses.
        /// </summary>
        /// <returns></returns>
        public SelectStatement ClearGroupBy()
        {
            this.groupByClauses.Clear();
            return this;
        }

        /// <summary>
        ///     Adds a column to the ORDER BY clause.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="asc">if set to <c>true</c> [asc].</param>
        /// <returns>The same statement for Fluentness</returns>
        public SelectStatement OrderBy(string column, bool asc = true)
        {
            this.orderByClauses.Add(new OrderByClause(column, asc));
            return this;
        }

        /// <summary>
        /// Clears the order by clauses.
        /// </summary>
        /// <returns>The same statement for Fluentness</returns>
        public SelectStatement ClearOrderBy()
        {
            this.orderByClauses.Clear();
            return this;
        }

        /// <summary>
        /// Adds a LIMIT clause.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>The same statement for Fluentness</returns>
        public SelectStatement Limit(string limit)
        {
            this.limitClause = limit;
            return this;
        }

        /// <summary>
        /// Adds a LIMIT clause.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>The same statement for Fluentness</returns>
        public SelectStatement Limit(int limit)
        {
            return this.Limit(limit.ToString());
        }

        /// <summary>
        /// Clears the limit clause
        /// </summary>
        /// <returns>The same statement for Fluentness</returns>
        public SelectStatement ClearLimit()
        {
            this.limitClause = null;
            return this;
        }

        /// <summary>
        /// Adds a OFFSET clause.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>The same statement for Fluentness</returns>
        public SelectStatement Offset(string offset)
        {
            this.offsetClause = offset;
            return this;
        }

        /// <summary>
        /// Adds a OFFSET clause.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>The same statement for Fluentness</returns>
        public SelectStatement Offset(int offset)
        {
            return this.Offset(offset.ToString());
        }

        /// <summary>
        /// Clears the offset clause
        /// </summary>
        /// <returns>The same statement for Fluentness</returns>
        public SelectStatement ClearOffset()
        {
            this.offsetClause = null;
            return this;
        }

        /// <summary>
        /// Returns the statement as an SQL string.
        /// </summary>
        /// <returns>The same statement for Fluentness</returns>
        public override void BuildSql(StringBuilder builder, ISqlRenderer renderer)
        {
            builder.Append(renderer.RenderSelect(this));
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public override SelectStatement Clone()
        {
            return new SelectStatement(this, this.columnsList.ToList(), this.groupByClauses.Select(o => o.Clone()).ToList(), this.orderByClauses.Select(o => o.Clone()).ToList(), this.limitClause, this.offsetClause);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public IFromClause CloneFrom()
        {
            return this.Clone();
        }

        /// <summary>
        /// Gets the alias of the from clause.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is complex and requires wrapping in a select statement.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is complex; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplex
        {
            get { return true; }
        }

        /// <summary>
        /// Sets the alias for this statement.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public SelectStatement As(string alias)
        {
            this.Alias = alias;
            return this;
        }

        /// <summary>
        /// Returns the equivalent statement for counting the full results without paging.
        /// </summary>
        /// <returns>A new SQL statement for counting all items.</returns>
        public SelectStatement ToCount()
        {
            return new SelectStatement(this, new List<string>() { "COUNT(*)" }, this.GroupByClauses.ToList(), new List<OrderByClause>(), null, null);
        }
    }
}
