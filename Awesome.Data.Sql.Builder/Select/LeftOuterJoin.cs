using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    public class LeftOuterJoin : JoinClause<LeftOuterJoin>
    {
        public LeftOuterJoin(FromClause firstTable, FromClause secondTable, string onClause)
            : base(firstTable, secondTable, onClause)
        {
        }

        protected override string ClauseName
        {
            get { return "LEFT OUTER"; }
        }

        public override LeftOuterJoin Clone()
        {
            return new LeftOuterJoin(this.FirstTable.CloneFrom(), this.SecondTable.CloneFrom(), this.OnClause);
        }
    }
}
