using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awesome.Data.Sql.Builder.Select;

namespace Awesome.Data.Sql.Builder.Renderers
{
    /// <summary>
    ///     A default SQL renderer.
    /// </summary>
    public class DefaultSqlRenderer : ISqlRenderer
    {
        /// <summary>
        ///     Indentation to use when indenting query sub-parts.
        /// </summary>
        private const string Indentation = "    ";

        /// <summary>
        ///     Separator for general things in SQL.
        /// </summary>
        private const string Separator = ", ";

        /// <summary>
        ///     Separator for general things in SQL at a line end.
        /// </summary>
        private const string SeparatorNoSpace = ",";

        /// <summary>
        /// Renders a LIMIT clause.
        /// </summary>
        /// <param name="limitClause">The limit clause.</param>
        /// <returns>A rendered SQL string.</returns>
        public virtual string Limit(string limitClause)
        {
            return string.Format("LIMIT {0}", limitClause);
        }

        /// <summary>
        /// Renders a OFFSET clause.
        /// </summary>
        /// <param name="offsetClause">The offset clause.</param>
        /// <returns>A rendered SQL string.</returns>
        public virtual string Offset(string offsetClause)
        {
            return string.Format("OFFSET {0}", offsetClause);
        }

        /// <summary>
        /// Renders a LIMIT and OFFSET clause.
        /// </summary>
        /// <param name="limitClause">The limit clause.</param>
        /// <param name="offsetClause">The offset clause.</param>
        public virtual string LimitOffset(string limitClause, string offsetClause)
        {
            return this.Limit(limitClause) + " " + this.Offset(offsetClause);
        }

        /// <summary>
        /// Renders columns for a select statement.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns>A rendered SQL string.</returns>
        public virtual string Columns(IEnumerable<string> columns)
        {
            return string.Join(Separator, columns);
        }

        public virtual string RenderSelect(SelectStatement select)
        {
            var builder = new StringBuilder();

            this.AppendSelect(builder, select);

            this.AppendFrom(builder, select);

            this.AppendWhere(builder, select);

            this.AppendGroupBy(builder, select);

            this.AppendOrderBy(builder, select);

            this.AppendLimitOffset(builder, select);

            return builder.ToString();
        }

        public virtual void AppendLimitOffset(StringBuilder builder, SelectStatement select)
        {
            if (!string.IsNullOrWhiteSpace(select.LimitClause) && !string.IsNullOrWhiteSpace(select.OffsetClause))
            {
                builder.Append(this.LimitOffset(select.LimitClause, select.OffsetClause));
            }
            else if (!string.IsNullOrWhiteSpace(select.LimitClause))
            {
                builder.Append(this.Limit(select.LimitClause));
            }
            else if (!string.IsNullOrWhiteSpace(select.OffsetClause))
            {
                builder.Append(this.Offset(select.OffsetClause));
            }
        }

        public virtual void AppendSelect(StringBuilder builder, SelectStatement select)
        {
            builder.AppendLine("SELECT");
            builder.AppendLine(Indentation + this.Columns(select.ColumnsList));
        }

        public virtual void AppendGroupBy(StringBuilder builder, SelectStatement select)
        {
            if (select.GroupByClauses.Any())
            {
                builder.AppendLine("GROUP BY");
                builder.AppendLine(Indentation + string.Join(Separator, select.GroupByClauses.Select(c => c.Column)));
            }
        }

        public virtual void AppendOrderBy(StringBuilder builder, SelectStatement select)
        {
            if (select.OrderByClauses.Any())
            {
                builder.AppendLine("ORDER BY");
                builder.AppendLine(Indentation + string.Join(Separator, select.OrderByClauses.Select(c => c.Column + " " + (c.Asc ? "ASC" : "DESC"))));
            }
        }

        /// <summary>
        /// Appends the from clause of the select statement to the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
        public virtual void AppendFrom(StringBuilder builder, SelectStatement select)
        {
            if (!select.Tables.Any())
            {
                return;
            }

            builder.AppendLine("FROM");
            int i = 0;
            foreach (var table in select.Tables)
            {
                builder.Append(Indentation);
                if (table.IsComplex)
                {
                    builder.AppendLine("(");
                    table.BuildSql(builder, this);
                    builder.Append("    )" + (!string.IsNullOrWhiteSpace(table.Alias) ? " " + table.Alias : string.Empty));
                }
                else
                {
                    table.BuildSql(builder, this);
                }

                if (i < select.Tables.Count - 1)
                {
                    builder.AppendLine(SeparatorNoSpace);
                }

                i++;
            }

            builder.AppendLine();
        }

        /// <summary>
        /// Appends the where clause of the select statement to the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
        public virtual void AppendWhere(StringBuilder builder, SelectStatement select)
        {
            if (!select.WhereClauses.Any())
            {
                return;
            }

            builder.AppendLine("WHERE");
            int i = 0;
            foreach (var clause in select.WhereClauses)
            {
                builder.Append(Indentation + clause.Clause);
                if (i < select.WhereClauses.Count - 1)
                {
                    builder.AppendLine(clause.Or ? " OR" : " AND");
                }
                else
                {
                    builder.AppendLine();
                }

                i++;
            }
        }
    }
}
