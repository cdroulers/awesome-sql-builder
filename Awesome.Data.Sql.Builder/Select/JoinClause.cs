using System.Text;
using Awesome.Data.Sql.Builder.Renderers;

namespace Awesome.Data.Sql.Builder.Select
{
    /// <summary>
    ///     An SQL Join clause.
    /// </summary>
    /// <typeparam name="T">Joining on which type.</typeparam>
    public abstract class JoinClause<T> : FromClause
        where T : JoinClause<T>
    {
        private readonly IFromClause firstTable;

        private readonly IFromClause secondTable;

        private readonly string onClause;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinClause&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="firstTable">The first table.</param>
        /// <param name="secondTable">The second table.</param>
        /// <param name="onClause">The on clause.</param>
        protected JoinClause(IFromClause firstTable, IFromClause secondTable, string onClause)
        {
            this.firstTable = firstTable;
            this.secondTable = secondTable;
            this.onClause = onClause;
        }

        /// <summary>
        /// Gets the name of the clause.
        /// </summary>
        protected abstract string ClauseName { get; }

        /// <summary>
        ///     The first clause to join on.
        /// </summary>
        public IFromClause FirstTable => this.firstTable;

        /// <summary>
        ///     The second clause to join on.
        /// </summary>
        public IFromClause SecondTable => this.secondTable;

        /// <summary>
        ///     The ON clause to join the two tables.
        /// </summary>
        public string OnClause => this.onClause;

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public abstract T Clone();

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public override IFromClause CloneFrom()
        {
            return this.Clone();
        }

        /// <summary>
        /// Returns the SQL for the current object.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="renderer">The SQL renderer to use.</param>
        public override void BuildSql(StringBuilder builder, ISqlRenderer renderer)
        {
            this.FirstTable.BuildSql(builder, renderer);
            builder.AppendLine();
            builder.Append("    " + this.ClauseName + " JOIN ");
            this.SecondTable.BuildSql(builder, renderer);
            builder.Append(" ON " + this.OnClause);
        }
    }
}
