using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
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
        public readonly ISetQuery First;
        /// <summary>
        ///     Second query
        /// </summary>
        public readonly ISetQuery Second;

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

        public void BuildFromSql(StringBuilder builder)
        {
            builder.AppendLine("(");
            this.BuildSql(builder);
            builder.Append(SelectStatement.Indentation + ")" + (!string.IsNullOrWhiteSpace(this.Alias) ? " " + this.Alias : string.Empty));
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
            this.First.BuildSql(builder);
            builder.AppendLine();
            builder.AppendLine(this.SetOperator);
            builder.AppendLine();
            this.Second.BuildSql(builder);
        }
    }
}
