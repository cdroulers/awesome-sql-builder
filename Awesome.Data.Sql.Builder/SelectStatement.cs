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

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectStatement"/> class.
        /// </summary>
        /// <param name="columns">The columns.</param>
        public SelectStatement(IEnumerable<string> columns)
        {
            this.columns = new List<string>(columns);
            this.orderByClauses = new List<OrderByClause>();
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

            return builder.ToString().Trim();
        }
    }
}
