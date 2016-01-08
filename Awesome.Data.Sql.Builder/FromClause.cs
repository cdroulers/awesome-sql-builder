using System;
using System.Text;
using Awesome.Data.Sql.Builder.Renderers;

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
        /// <param name="builder">The string builder</param>
        public void BuildSql(StringBuilder builder)
        {
            this.BuildSql(builder, new DefaultSqlRenderer());
        }

        /// <summary>
        /// Returns the SQL for the current object.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="renderer">The SQL renderer to use.</param>
        public abstract void BuildSql(StringBuilder builder, ISqlRenderer renderer);

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
        /// <param name="renderer">The renderer.</param>
        public void BuildFromSql(StringBuilder builder, ISqlRenderer renderer)
        {
            this.BuildSql(builder, renderer);
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
            get { return false; }
        }
    }
}
