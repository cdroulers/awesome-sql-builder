using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    /// <summary>
    ///     Represents an SQL statement
    /// </summary>
    public class SelectStatement : SqlStatement<SelectStatement>
    {
        private readonly List<string> columnsList;
        /// <summary>
        /// Returns
        /// </summary>
        public ReadOnlyCollection<string> ColumnsList { get { return new ReadOnlyCollection<string>(this.columnsList); } }
        private readonly List<OrderByClause> orderByClauses;
        /// <summary>
        /// Returns a list of order by clauses.
        /// </summary>
        public ReadOnlyCollection<OrderByClause> OrderByClauses { get { return new ReadOnlyCollection<OrderByClause>(this.orderByClauses); } }
        private string limitClause;
        /// <summary>
        /// Gets the limit clause
        /// </summary>
        public string LimitClause { get { return this.limitClause; } }
        private string offsetClause;
        /// <summary>
        /// Gets the offset clause
        /// </summary>
        public string OffsetClause { get { return this.offsetClause; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectStatement"/> class.
        /// </summary>
        /// <param name="columns">The columns.</param>
        public SelectStatement(IEnumerable<string> columns)
        {
            this.columnsList = new List<string>(columns);
            this.orderByClauses = new List<OrderByClause>();
        }

        private SelectStatement(SqlStatement<SelectStatement> statement, List<string> columns, List<OrderByClause> orderByClauses, string limitClause, string offsetClause)
            : base(statement)
        {
            this.columnsList = columns;
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
        ///     Adds a column to the ORDER BY clause.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="asc">if set to <c>true</c> [asc].</param>
        /// <param name="clearCurrent">if set to <c>true</c> [clearCurrent]</param>
        /// <returns></returns>
        public SelectStatement OrderBy(string column, bool asc = true, bool clearCurrent = false)
        {
            if (clearCurrent)
            {
                this.orderByClauses.Clear();
            }
            this.orderByClauses.Add(new OrderByClause(column, asc));
            return this;
        }

        /// <summary>
        /// Clears the order by clauses.
        /// </summary>
        /// <returns></returns>
        public SelectStatement ClearOrderBy()
        {
            this.orderByClauses.Clear();
            return this;
        }

        /// <summary>
        /// Adds a LIMIT clause.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public SelectStatement Limit(string limit)
        {
            this.limitClause = limit;
            return this;
        }

        /// <summary>
        /// Adds a LIMIT clause.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public SelectStatement Limit(int limit)
        {
            return this.Limit(limit.ToString());
        }

        /// <summary>
        /// Clears the limit clause
        /// </summary>
        /// <returns></returns>
        public SelectStatement ClearLimit()
        {
            this.limitClause = null;
            return this;
        }

        /// <summary>
        /// Adds a OFFSET clause.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public SelectStatement Offset(string offset)
        {
            this.offsetClause = offset;
            return this;
        }

        /// <summary>
        /// Adds a OFFSET clause.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public SelectStatement Offset(int offset)
        {
            return this.Offset(offset.ToString());
        }

        /// <summary>
        /// Clears the offset clause
        /// </summary>
        /// <returns></returns>
        public SelectStatement ClearOffset()
        {
            this.offsetClause = null;
            return this;
        }

        /// <summary>
        /// Performs an OUTER JOIN.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="onClause">The on clause.</param>
        /// <returns></returns>
        public SelectStatement OuterJoin(FromClause table, string onClause)
        {
            this.TransformLastTable(lastClause => new OuterJoin(lastClause, table, onClause));
            return this;
        }

        /// <summary>
        /// Performs an INNER JOIN.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="onClause">The on clause.</param>
        /// <returns></returns>
        public SelectStatement InnerJoin(FromClause table, string onClause)
        {
            this.TransformLastTable(lastClause => new InnerJoin(lastClause, table, onClause));
            return this;
        }

        /// <summary>
        /// Performs a LEFT OUTER JOIN.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="onClause">The on clause.</param>
        /// <returns></returns>
        public SelectStatement LeftOuterJoin(FromClause table, string onClause)
        {
            this.TransformLastTable(lastClause => new LeftOuterJoin(lastClause, table, onClause));
            return this;
        }

        /// <summary>
        /// Performs a RIGHT OUTER JOIN.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="onClause">The on clause.</param>
        /// <returns></returns>
        public SelectStatement RightOuterJoin(FromClause table, string onClause)
        {
            this.TransformLastTable(lastClause => new RightOuterJoin(lastClause, table, onClause));
            return this;
        }

        /// <summary>
        /// Performs a FULL JOIN.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="onClause">The on clause.</param>
        /// <returns></returns>
        public SelectStatement FullJoin(FromClause table, string onClause)
        {
            this.TransformLastTable(lastClause => new FullJoin(lastClause, table, onClause));
            return this;
        }

        /// <summary>
        /// Returns the statement as an SQL string.
        /// </summary>
        /// <returns></returns>
        public override void BuildSql(StringBuilder builder)
        {
            builder.AppendLine("SELECT");
            builder.AppendLine(Indentation + string.Join(Separator, this.columnsList));
            this.AppendFrom(builder);
            this.AppendWhere(builder);

            if (this.orderByClauses.Any())
            {
                builder.AppendLine("ORDER BY");
                builder.AppendLine(Indentation + string.Join(Separator, this.orderByClauses.Select(c => c.Column + " " + (c.Asc ? "ASC" : "DESC"))));
            }

            if (!string.IsNullOrWhiteSpace(this.limitClause))
            {
                builder.AppendFormat("LIMIT {0} ", this.limitClause);
            }
            if (!string.IsNullOrWhiteSpace(this.offsetClause))
            {
                builder.AppendFormat("OFFSET {0} ", this.offsetClause);
            }
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override SelectStatement Clone()
        {
            return new SelectStatement(this, this.columnsList.ToList(), this.orderByClauses.Select(o => o.Clone()).ToList(), this.limitClause, this.offsetClause);
        }
    }
}
