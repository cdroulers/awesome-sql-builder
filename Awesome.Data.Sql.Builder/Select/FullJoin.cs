using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    /// <summary>
    ///     An SQL Full join
    /// </summary>
    public class FullJoin : JoinClause<FullJoin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FullJoin"/> class.
        /// </summary>
        /// <param name="firstTable">The first table.</param>
        /// <param name="secondTable">The second table.</param>
        /// <param name="onClause">The on clause.</param>
        public FullJoin(FromClause firstTable, FromClause secondTable, string onClause)
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
            get { return "FULL"; }
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override FullJoin Clone()
        {
            return new FullJoin(this.FirstTable.CloneFrom(), this.SecondTable.CloneFrom(), this.OnClause);
        }
    }
}
