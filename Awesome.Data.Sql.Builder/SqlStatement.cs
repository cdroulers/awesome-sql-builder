using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Awesome.Data.Sql.Builder.Renderers;

namespace Awesome.Data.Sql.Builder
{
    /// <summary>
    ///     The basics of an SQL statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SqlStatement<T> : ICloneable, ISqlFragment
        where T : SqlStatement<T>
    {
        private readonly List<IFromClause> tables;
        private readonly List<WhereClause> whereClauses;

        /// <summary>
        ///     A list of tables to query.
        /// </summary>
        public ReadOnlyCollection<IFromClause> Tables
        {
            get { return new ReadOnlyCollection<IFromClause>(this.tables); }
        }

        /// <summary>
        /// A list of where clauses
        /// </summary>
        public ReadOnlyCollection<WhereClause> WhereClauses
        {
            get { return new ReadOnlyCollection<WhereClause>(this.whereClauses); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlStatement&lt;T&gt;"/> class.
        /// </summary>
        protected SqlStatement()
        {
            this.tables = new List<IFromClause>();
            this.whereClauses = new List<WhereClause>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlStatement&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="statement">The statement.</param>
        protected SqlStatement(SqlStatement<T> statement)
        {
            this.tables = statement.tables.ToList();
            this.whereClauses = statement.whereClauses.Select(w => w.Clone()).ToList();
        }

        /// <summary>
        ///     Adds a table to the FROM list of the statement.
        /// </summary>
        /// <param name="newTables">The tables.</param>
        /// <returns>The current instance for fluentness</returns>
        public T From(params string[] newTables)
        {
            return this.From(newTables.Select(t => new TableClause(t)).ToArray<IFromClause>());
        }

        /// <summary>
        /// Adds tables to the FROM list of the statement.
        /// </summary>
        /// <param name="newTables">The tables.</param>
        /// <returns></returns>
        public T From(params IFromClause[] newTables)
        {
            this.tables.AddRange(newTables);
            return (T)this;
        }

        /// <summary>
        /// Transforms the last table with a function.
        /// </summary>
        /// <param name="transform">The transform.</param>
        protected void TransformLastTable(Func<IFromClause, IFromClause> transform)
        {
            if (!this.tables.Any())
            {
                throw new InvalidOperationException(Awesome.Data.Sql.Builder.Properties.Strings.CannotTransformAnEmptyFromClause);
            }

            this.tables[this.tables.Count - 1] = transform(this.tables[this.tables.Count - 1]);
        }

        /// <summary>
        ///     Adds a WHERE clause to the statement
        /// </summary>
        /// <param name="clause">The clause.</param>
        /// <param name="or">if set to <c>true</c> [or].</param>
        /// <returns></returns>
        public T Where(string clause, bool or = false)
        {
            this.whereClauses.Add(new WhereClause(clause, or));
            return (T)this;
        }

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
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.ToSql();
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
        /// <returns></returns>
        public abstract T Clone();
    }
}
