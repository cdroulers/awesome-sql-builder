using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    public class RightOuterJoin : JoinClause<RightOuterJoin>
    {
        public RightOuterJoin(FromClause firstTable, FromClause secondTable, string onClause)
            : base(firstTable, secondTable, onClause)
        {
        }

        protected override string ClauseName
        {
            get { return "RIGHT OUTER"; }
        }

        public override RightOuterJoin Clone()
        {
            return new RightOuterJoin(this.FirstTable.CloneFrom(), this.SecondTable.CloneFrom(), this.OnClause);
        }
    }
}
