using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
{
    public class OrderByClause
    {
        public readonly string Column;
        public readonly bool Asc;

        public OrderByClause(string column, bool asc)
        {
            this.Column = column;
            this.Asc = asc;
        }
    }
}
