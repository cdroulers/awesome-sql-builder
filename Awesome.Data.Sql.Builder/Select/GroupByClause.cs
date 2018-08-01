using System;

namespace Awesome.Data.Sql.Builder.Select
{
    /// <summary>
    ///     Represents an GROUP BY column in an SQL statement.
    /// </summary>
    public class GroupByClause : ICloneable
    {
        private readonly string column;

        /// <summary>
        ///     The column to group by.
        /// </summary>
        public string Column => this.column;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupByClause"/> class.
        /// </summary>
        /// <param name="column">The column.</param>
        public GroupByClause(string column)
        {
            this.column = column;
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
        public GroupByClause Clone()
        {
            return new GroupByClause(this.Column);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="GroupByClause"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator GroupByClause(string s)
        {
            return new GroupByClause(s);
        }
    }
}
