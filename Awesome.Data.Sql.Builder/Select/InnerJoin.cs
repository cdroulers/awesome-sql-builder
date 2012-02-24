using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    public class InnerJoin : JoinClause<InnerJoin>
    {
        public InnerJoin(FromClause firstTable, FromClause secondTable, string onClause)
            : base(firstTable, secondTable, onClause)
        {
        }

        protected override string ClauseName
        {
            get { return "INNER"; }
        }

        public override InnerJoin Clone()
        {
            return new InnerJoin(this.FirstTable.CloneFrom(), this.SecondTable.CloneFrom(), this.OnClause);
        }
    }
}
