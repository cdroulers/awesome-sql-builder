using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    /// <summary>
    /// An SQL Left Outer Join
    /// </summary>
    public class LeftOuterJoin : JoinClause<LeftOuterJoin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeftOuterJoin"/> class.
        /// </summary>
        /// <param name="firstTable">The first table.</param>
        /// <param name="secondTable">The second table.</param>
        /// <param name="onClause">The on clause.</param>
        public LeftOuterJoin(FromClause firstTable, FromClause secondTable, string onClause)
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
            get { return "LEFT OUTER"; }
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override LeftOuterJoin Clone()
        {
            return new LeftOuterJoin(this.FirstTable.CloneFrom(), this.SecondTable.CloneFrom(), this.OnClause);
        }
    }
}
