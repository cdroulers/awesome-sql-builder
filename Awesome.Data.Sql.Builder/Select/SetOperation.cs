using System;
using System.Text;
using Awesome.Data.Sql.Builder.Renderers;

namespace Awesome.Data.Sql.Builder.Select
{
    /// <summary>
    ///     An SQL SET operation
    ///     <remarks>http://en.wikipedia.org/wiki/Set_operations_(SQL)</remarks>
    /// </summary>
    public abstract class SetOperation<T> : ISetQuery
        where T : SetOperation<T>
    {
        /// <summary>
        ///     First query
        /// </summary>
        protected readonly ISetQuery First;

        /// <summary>
        ///     Second query
        /// </summary>
        protected readonly ISetQuery Second;

        /// <summary>
        ///     Gets the set operator of the operation
        /// </summary>
        protected abstract string SetOperator { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetOperation&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        protected SetOperation(ISetQuery first, ISetQuery second)
        {
            this.First = first;
            this.Second = second;
        }

        /// <summary>
        /// Adds the SQL for the current object to the builder
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="renderer">The renderer.</param>
        public void BuildFromSql(StringBuilder builder, ISqlRenderer renderer)
        {
            builder.AppendLine("(");
            this.BuildSql(builder, renderer);
            builder.Append("    )" + (!string.IsNullOrWhiteSpace(this.Alias) ? " " + this.Alias : string.Empty));
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public IFromClause CloneFrom()
        {
            return this.Clone();
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
            get { return true; }
        }

        /// <summary>
        /// Sets the alias for this statement.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public T As(string alias)
        {
            this.Alias = alias;
            return (T)this;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public abstract T Clone();

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
        /// Adds the SQL for the current object to the builder
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void BuildSql(StringBuilder builder)
        {
            this.BuildSql(builder, new DefaultSqlRenderer());
        }

        /// <summary>
        /// Adds the SQL for the current object to the builder
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="renderer">The SQL renderer to use.</param>
        public void BuildSql(StringBuilder builder, ISqlRenderer renderer)
        {
            this.First.BuildSql(builder, renderer);
            builder.AppendLine();
            builder.AppendLine(this.SetOperator);
            builder.AppendLine();
            this.Second.BuildSql(builder, renderer);
        }
    }
}
