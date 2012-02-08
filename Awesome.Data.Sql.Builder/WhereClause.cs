using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
{
    /// <summary>
    ///     Represents an SQL where clause in an SQL statement.
    /// </summary>
    public class WhereClause
    {
        /// <summary>
        ///     The clause of the WHERE statement
        /// </summary>
        public readonly string Clause;
        /// <summary>
        ///     If the condition between this clause and the next is an SQL OR or AND
        /// </summary>
        public readonly bool Or;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereClause"/> class.
        /// </summary>
        /// <param name="clause">The clause.</param>
        /// <param name="or">if set to <c>true</c> [or].</param>
        public WhereClause(string clause, bool or)
        {
            this.Clause = clause;
            this.Or = or;
        }
    }
}
