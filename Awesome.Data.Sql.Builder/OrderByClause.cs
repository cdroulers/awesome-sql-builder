using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
{
    /// <summary>
    ///     Represents an ORDER BY column in an SQL statement.
    /// </summary>
    public class OrderByClause
    {
        /// <summary>
        ///     The column to order by
        /// </summary>
        public readonly string Column;
        /// <summary>
        ///     Whether to order in ascending or descending order.
        /// </summary>
        public readonly bool Asc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByClause"/> class.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="asc">if set to <c>true</c> [asc].</param>
        public OrderByClause(string column, bool asc)
        {
            this.Column = column;
            this.Asc = asc;
        }
    }
}
