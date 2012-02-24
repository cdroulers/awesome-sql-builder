using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
{
    /// <summary>
    ///     Represents FROM table or join or sub-select in an SQL statement.
    /// </summary>
    public abstract class FromClause : ICloneable, ISqlFragment
    {
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return this.CloneFrom();
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public abstract FromClause CloneFrom();

        /// <summary>
        /// Returns the SQL for the current object.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public abstract void BuildSql(StringBuilder builder);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="System.Data.Sql.Builder.FromClause"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator FromClause(string s)
        {
            return new TableClause(s);
        }
    }
}
