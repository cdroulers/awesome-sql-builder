using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
{
    public class SelectStatement : SqlStatement<SelectStatement>, ISqlStatement<SelectStatement>
    {
        private readonly List<string> columns;
        private readonly List<OrderByClause> orderByClauses;

        public SelectStatement(string[] columns)
        {
            this.columns = new List<string>(columns);
            this.orderByClauses = new List<OrderByClause>();
        }

        public SelectStatement OrderBy(string column, bool asc = true)
        {
            this.orderByClauses.Add(new OrderByClause(column, asc));
            return this;
        }

        public override string ToSql()
        {
            var builder = new StringBuilder("SELECT");
            builder.AppendLine();
            builder.AppendLine("    " + string.Join(", ", this.columns));
            builder.AppendLine("FROM");
            builder.AppendLine("    " + this.GetTables());
            this.AppendWhere(builder);

            if (this.orderByClauses.Any())
            {
                builder.AppendLine("ORDER BY");
                builder.AppendLine("    " + string.Join(", ", this.orderByClauses.Select(c => c.Column + " " + (c.Asc ? "ASC" : "DESC"))));
            }

            return builder.ToString().Trim();
        }
    }
}
