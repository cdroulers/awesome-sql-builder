using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awesome.Data.Sql.Builder.Insert;
using Awesome.Data.Sql.Builder.Select;
using Awesome.Data.Sql.Builder.Update;

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

        /// <summary>
        /// Renders the select.
        /// </summary>
        /// <param name="select">The select.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Appends the limit offset.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
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

        /// <summary>
        /// Appends the select.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
        public virtual void AppendSelect(StringBuilder builder, SelectStatement select)
        {
            builder.AppendLine("SELECT");
            builder.AppendLine(Indentation + this.Columns(select.ColumnsList));
        }

        /// <summary>
        /// Appends the update statement.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="update">The update statement.</param>
        public virtual void AppendUpdate(StringBuilder builder, UpdateStatement update)
        {
            builder.Append("UPDATE");
            if (string.IsNullOrWhiteSpace(update.TableToUpdate))
            {
                builder.Append(" ");
                update.Tables.First().BuildSql(builder, this);
            }
            else
            {
                builder.Append(" " + update.TableToUpdate);
            }

            builder.AppendLine();
            this.AppendUpdateColumns(builder, update);

            if (!string.IsNullOrWhiteSpace(update.TableToUpdate))
            {
                this.AppendFrom(builder, update);
            }
        }

        /// <summary>
        /// Appends the update statement columns' SET clauses
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="update">The update statement.</param>
        public virtual void AppendUpdateColumns(StringBuilder builder, UpdateStatement update)
        {
            builder.AppendLine("SET");
            var i = 0;
            foreach (var column in update.ColumnsList)
            {
                builder.Append(Indentation + column + " = @" + column);

                if (i < update.ColumnsList.Count - 1)
                {
                    builder.AppendLine(SeparatorNoSpace);
                }

                i++;
            }

            builder.AppendLine();
        }

        /// <summary>
        /// Appends the group by.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
        public virtual void AppendGroupBy(StringBuilder builder, SelectStatement select)
        {
            if (select.GroupByClauses.Any())
            {
                builder.AppendLine("GROUP BY");
                builder.AppendLine(Indentation + string.Join(Separator, select.GroupByClauses.Select(c => c.Column)));
            }
        }

        /// <summary>
        /// Appends the order by.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
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
        /// <param name="statement">The statement.</param>
        public virtual void AppendFrom<T>(StringBuilder builder, SqlStatement<T> statement)
            where T : SqlStatement<T>
        {
            if (!statement.Tables.Any())
            {
                return;
            }

            builder.AppendLine("FROM");
            int i = 0;
            foreach (var table in statement.Tables)
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

                if (i < statement.Tables.Count - 1)
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
        /// <param name="statement">The whereable statement.</param>
        public virtual void AppendWhere<T>(StringBuilder builder, SqlStatement<T> statement)
            where T : SqlStatement<T>
        {
            if (!statement.WhereClauses.Any())
            {
                return;
            }

            builder.AppendLine("WHERE");
            int i = 0;
            foreach (var clause in statement.WhereClauses)
            {
                builder.Append(Indentation + clause.Clause);
                if (i < statement.WhereClauses.Count - 1)
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

        /// <summary>
        /// Renders an Update statement to a string.
        /// </summary>
        /// <param name="update">An UPDATE statement</param>
        /// <returns>An SQL String for the UPDATE.</returns>
        public string RenderUpdate(UpdateStatement update)
        {
            var builder = new StringBuilder();

            this.AppendUpdate(builder, update);

            this.AppendWhere(builder, update);

            return builder.ToString();
        }

        /// <summary>
        /// Renders an Insert statement to a string.
        /// </summary>
        /// <param name="insert">An INSERT statement</param>
        /// <returns>An SQL String for the INSERT.</returns>
        public string RenderInsert(InsertStatement insert)
        {
            var builder = new StringBuilder();
            const string doubleIndentation = Indentation + Indentation;

            builder.AppendLine("INSERT INTO " + insert.Table); if (insert.Select != null)
            {
                // Columns list
                builder.AppendLine(Indentation + "(");
                builder.AppendLine(
                    doubleIndentation +
                    string.Join(
                        SeparatorNoSpace + Environment.NewLine + doubleIndentation,
                        insert.Select.ColumnsList));
                builder.AppendLine(Indentation + ")");

                // Select
                builder.Append(this.RenderSelect(insert.Select));
            }
            else if (insert.ColumnsList.Any())
            {
                // Columns list
                builder.AppendLine(Indentation + "(");
                builder.AppendLine(
                    doubleIndentation +
                    string.Join(
                        SeparatorNoSpace + Environment.NewLine + doubleIndentation,
                        insert.ColumnsList));
                builder.AppendLine(Indentation + ")");

                // Rows
                var rowCount = insert.RowCount.GetValueOrDefault(1);
                builder.AppendLine("VALUES");
                for (int i = 0; i < rowCount; i++)
                {
                    builder.AppendLine(Indentation + "(");
                    builder.AppendLine(
                        doubleIndentation +
                        string.Join(
                            SeparatorNoSpace + Environment.NewLine + doubleIndentation,
                            insert.ColumnsList.Select(x =>
                                "@" +
                                x +
                                (rowCount == 1 ? string.Empty : i.ToString()))));
                    builder.Append(Indentation + ")");

                    if (i < rowCount - 1)
                    {
                        builder.AppendLine(",");
                    }
                }
            }
            else
            {
                throw new NotSupportedException("Cannot render INSERT statement with no columns or no SELECT statement.");
            }

            return builder.ToString();
        }
    }
}
