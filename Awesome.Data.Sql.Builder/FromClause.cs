using System;
using System.Text;

namespace Awesome.Data.Sql.Builder
{
    /// <summary>
    ///     Represents FROM table or join or sub-select in an SQL statement.
    /// </summary>
    public abstract class FromClause : IFromClause
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
        public abstract IFromClause CloneFrom();

        /// <summary>
        /// Returns the SQL for the current object.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public abstract void BuildSql(StringBuilder builder);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="FromClause"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator FromClause(string s)
        {
            return new TableClause(s);
        }

        /// <summary>
        /// Adds the SQL for the current object to the builder
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void BuildFromSql(StringBuilder builder)
        {
            this.BuildSql(builder);
        }

        /// <summary>
        /// Gets the alias of the from clause.
        /// </summary>
        public string Alias { get; set; }
    }
}
