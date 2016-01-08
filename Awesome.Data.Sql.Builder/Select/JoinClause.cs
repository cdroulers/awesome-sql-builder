using System.Text;

namespace Awesome.Data.Sql.Builder.Select
{
    /// <summary>
    ///     An SQL Join clause.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class JoinClause<T> : FromClause
        where T : JoinClause<T>
    {
        /// <summary>
        ///     The first clause to join on.
        /// </summary>
        public readonly IFromClause FirstTable;

        /// <summary>
        ///     The second clause to join on.
        /// </summary>
        public readonly IFromClause SecondTable;

        /// <summary>
        ///     The ON clause to join the two tables.
        /// </summary>
        public readonly string OnClause;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinClause&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="firstTable">The first table.</param>
        /// <param name="secondTable">The second table.</param>
        /// <param name="onClause">The on clause.</param>
        protected JoinClause(IFromClause firstTable, IFromClause secondTable, string onClause)
        {
            this.FirstTable = firstTable;
            this.SecondTable = secondTable;
            this.OnClause = onClause;
        }

        /// <summary>
        /// Gets the name of the clause.
        /// </summary>
        protected abstract string ClauseName { get; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public abstract T Clone();

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override IFromClause CloneFrom()
        {
            return this.Clone();
        }

        /// <summary>
        /// Returns the SQL for the current object.
        /// </summary>
        /// <param name="builder"></param>
        public override void BuildSql(StringBuilder builder)
        {
            this.FirstTable.BuildSql(builder);
            builder.AppendLine();
            builder.Append(SelectStatement.Indentation + this.ClauseName + " JOIN ");
            this.SecondTable.BuildSql(builder);
            builder.Append(" ON " + this.OnClause);
        }
    }
}
