namespace Awesome.Data.Sql.Builder.Select
{
    /// <summary>
    ///     An SQL Inner Join
    /// </summary>
    public class InnerJoin : JoinClause<InnerJoin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InnerJoin"/> class.
        /// </summary>
        /// <param name="firstTable">The first table.</param>
        /// <param name="secondTable">The second table.</param>
        /// <param name="onClause">The on clause.</param>
        public InnerJoin(IFromClause firstTable, IFromClause secondTable, string onClause)
            : base(firstTable, secondTable, onClause)
        {
        }

        /// <summary>
        /// Gets the name of the clause.
        /// </summary>
        /// <value>
        /// The name of the clause.
        /// </value>
        protected override string ClauseName
        {
            get { return "INNER"; }
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override InnerJoin Clone()
        {
            return new InnerJoin(this.FirstTable.CloneFrom(), this.SecondTable.CloneFrom(), this.OnClause);
        }
    }
}
