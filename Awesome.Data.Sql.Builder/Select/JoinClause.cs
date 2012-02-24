using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    public abstract class JoinClause<T> : FromClause
        where T : JoinClause<T>
    {
        public readonly FromClause FirstTable;
        public readonly FromClause SecondTable;
        public readonly string OnClause;

        protected JoinClause(FromClause firstTable, FromClause secondTable, string onClause)
        {
            this.FirstTable = firstTable;
            this.SecondTable = secondTable;
            this.OnClause = onClause;
        }

        protected abstract string ClauseName { get; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public abstract T Clone();

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override FromClause CloneFrom()
        {
            return this.Clone();
        }

        public override void BuildSql(StringBuilder builder)
        {
            this.FirstTable.BuildSql(builder);
            builder.AppendLine();
            builder.Append(SelectStatement.Indentation + this.ClauseName + " JOIN ");
            this.SecondTable.BuildSql(builder);
            builder.Append(" ON " + this.OnClause);
        }
    }
}
