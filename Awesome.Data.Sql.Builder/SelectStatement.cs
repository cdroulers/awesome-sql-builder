using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
{
    /// <summary>
    ///     Represents an SQL statement
    /// </summary>
    public class SelectStatement : SqlStatement<SelectStatement>
    {
        private readonly List<string> columns;
        private readonly List<OrderByClause> orderByClauses;
        private string limitClause;
        private string offsetClause;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectStatement"/> class.
        /// </summary>
        /// <param name="columns">The columns.</param>
        public SelectStatement(IEnumerable<string> columns)
        {
            this.columns = new List<string>(columns);
            this.orderByClauses = new List<OrderByClause>();
        }

        private SelectStatement(SqlStatement<SelectStatement> statement, List<string> columns, List<OrderByClause> orderByClauses, string limitClause, string offsetClause)
            : base(statement)
        {
            this.columns = columns;
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
                this.columns.Clear();
            }
            this.columns.AddRange(columns);
            return this;
        }

        /// <summary>
        ///     Adds a column to the ORDER BY clause.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="asc">if set to <c>true</c> [asc].</param>
        /// <returns></returns>
        public SelectStatement OrderBy(string column, bool asc = true)
        {
            this.orderByClauses.Add(new OrderByClause(column, asc));
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
        /// Returns the statement as an SQL string.
        /// </summary>
        /// <returns></returns>
        public override string ToSql()
        {
            var builder = new StringBuilder("SELECT");
            builder.AppendLine();
            builder.AppendLine(Indentation + string.Join(Separator, this.columns));
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

            return builder.ToString().Trim();
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override SelectStatement Clone()
        {
            return new SelectStatement(this, this.columns.ToList(), this.orderByClauses.Select(o => o.Clone()).ToList(), this.limitClause, this.offsetClause);
        }
    }
}
