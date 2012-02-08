using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
{
    public abstract class SqlStatement<T>
        where T : SqlStatement<T>
    {
        private readonly List<string> tables;
        private readonly List<WhereClause> whereClauses;

        protected SqlStatement()
        {
            this.tables = new List<string>();
            this.whereClauses = new List<WhereClause>();
        }

        public T From(params string[] tables)
        {
            this.tables.AddRange(tables);
            return (T)this;
        }

        public T Where(string clause, bool or = false)
        {
            this.whereClauses.Add(new WhereClause(clause, or));
            return (T)this;
        }

        public abstract string ToSql();

        public override string ToString()
        {
            return this.ToSql();
        }

        protected string GetTables()
        {
            return string.Join(", ", this.tables);
        }

        protected void AppendWhere(StringBuilder builder)
        {
            if (this.whereClauses.Any())
            {
                builder.AppendLine("WHERE");
                int i = 0;
                foreach (var clause in this.whereClauses)
                {
                    builder.Append("    " + clause.Clause);
                    if (i < this.whereClauses.Count - 1) // Last clause
                    {
                        builder.AppendLine(" AND");
                    }
                    else
                    {
                        builder.AppendLine();
                    }
                    i++;
                }
            }
        }

        public static SelectStatement Select(params string[] columns)
        {
            return new SelectStatement(columns);
        }
    }
}
