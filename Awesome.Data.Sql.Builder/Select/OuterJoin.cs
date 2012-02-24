using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    public class OuterJoin : JoinClause<OuterJoin>
    {
        public OuterJoin(FromClause firstTable, FromClause secondTable, string onClause)
            : base(firstTable, secondTable, onClause)
        {
        }

        protected override string ClauseName
        {
            get { return "OUTER"; }
        }

        public override OuterJoin Clone()
        {
            return new OuterJoin(this.FirstTable.CloneFrom(), this.SecondTable.CloneFrom(), this.OnClause);
        }
    }
}
