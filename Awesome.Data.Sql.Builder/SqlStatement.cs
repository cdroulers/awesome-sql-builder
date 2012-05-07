using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace System.Data.Sql.Builder
{
    /// <summary>
    ///     The basics of an SQL statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SqlStatement<T> : ICloneable, ISqlFragment
        where T : SqlStatement<T>
    {
        /// <summary>
        ///     Indentation to use when indenting query sub-parts.
        /// </summary>
        internal const string Indentation = "    ";
        /// <summary>
        ///     Separator for general things in SQL.
        /// </summary>
        protected const string Separator = ", ";
        /// <summary>
        ///     Separator for general things in SQL at a line end.
        /// </summary>
        protected const string SeparatorNoSpace = ",";

        private readonly List<FromClause> tables;
        /// <summary>
        ///     A list of tables to query.
        /// </summary>
        public ReadOnlyCollection<FromClause> Tables { get { return new ReadOnlyCollection<FromClause>(this.tables); } }
        private readonly List<WhereClause> whereClauses;
        /// <summary>
        /// A list of where clauses
        /// </summary>
        public ReadOnlyCollection<WhereClause> WhereClauses { get { return new ReadOnlyCollection<WhereClause>(this.whereClauses); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlStatement&lt;T&gt;"/> class.
        /// </summary>
        protected SqlStatement()
        {
            this.tables = new List<FromClause>();
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
        /// <param name="tables">The tables.</param>
        /// <returns></returns>
        public T From(params string[] tables)
        {
            return this.From(tables.Select(t => new TableClause(t)).ToArray());
        }

        /// <summary>
        /// Adds tables to the FROM list of the statement.
        /// </summary>
        /// <param name="tables">The tables.</param>
        /// <returns></returns>
        public T From(params FromClause[] tables)
        {
            this.tables.AddRange(tables);
            return (T)this;
        }

        /// <summary>
        /// Transforms the last table with a function.
        /// </summary>
        /// <param name="transform">The transform.</param>
        protected void TransformLastTable(Func<FromClause, FromClause> transform)
        {
            if (!this.tables.Any())
            {
                throw new InvalidOperationException(Properties.Strings.CannotTransformAnEmptyFromClause);
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
        /// <param name="builder"></param>
        /// <returns></returns>
        public abstract void BuildSql(StringBuilder builder);

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
        ///     Appends the FROM statement to the builder.
        /// </summary>
        /// <returns></returns>
        protected void AppendFrom(StringBuilder builder)
        {
            if (this.tables.Any())
            {
                builder.AppendLine("FROM");
                int i = 0;
                foreach (var table in this.tables)
                {
                    builder.Append(Indentation);
                    table.BuildSql(builder);
                    if (i < this.tables.Count - 1) // Not last clause
                    {
                        builder.AppendLine(SeparatorNoSpace);
                    }
                    i++;
                }
                builder.AppendLine();
            }
        }

        /// <summary>
        ///     Appends the WHERE statement to the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected void AppendWhere(StringBuilder builder)
        {
            if (this.whereClauses.Any())
            {
                builder.AppendLine("WHERE");
                int i = 0;
                foreach (var clause in this.whereClauses)
                {
                    builder.Append(Indentation + clause.Clause);
                    if (i < this.whereClauses.Count - 1) // Not last clause
                    {
                        builder.AppendLine(clause.Or ? " OR" : " AND");
                    }
                    else
                    {
                        builder.AppendLine();
                    }
                    i++;
                }
            }
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
