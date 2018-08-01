using System;

namespace Awesome.Data.Sql.Builder
{
    /// <summary>
    ///     Represents an SQL where clause in an SQL statement.
    /// </summary>
    public class WhereClause : ICloneable
    {
        private readonly string clause;

        private readonly bool or;

        /// <summary>
        ///     The clause of the WHERE statement.
        /// </summary>
        public string Clause => this.clause;

        /// <summary>
        ///     If the condition between this clause and the next is an SQL OR or AND.
        /// </summary>
        public bool Or => this.or;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereClause"/> class.
        /// </summary>
        /// <param name="clause">The clause.</param>
        /// <param name="or">if set to <c>true</c> [or].</param>
        public WhereClause(string clause, bool or)
        {
            this.clause = clause;
            this.or = or;
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
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public WhereClause Clone()
        {
            return new WhereClause(this.Clause, this.Or);
        }
    }
}
