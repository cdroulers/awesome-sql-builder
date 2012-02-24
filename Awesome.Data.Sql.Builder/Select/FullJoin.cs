using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    public class FullJoin : JoinClause<FullJoin>
    {
        public FullJoin(FromClause firstTable, FromClause secondTable, string onClause)
            : base(firstTable, secondTable, onClause)
        {
        }

        protected override string ClauseName
        {
            get { return "FULL"; }
        }

        public override FullJoin Clone()
        {
            return new FullJoin(this.FirstTable.CloneFrom(), this.SecondTable.CloneFrom(), this.OnClause);
        }
    }
}
