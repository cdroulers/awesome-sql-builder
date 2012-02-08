using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
{
    public class WhereClause
    {
        public readonly string Clause;
        public readonly bool Or;

        public WhereClause(string clause, bool or)
        {
            this.Clause = clause;
            this.Or = or;
        }
    }
}
