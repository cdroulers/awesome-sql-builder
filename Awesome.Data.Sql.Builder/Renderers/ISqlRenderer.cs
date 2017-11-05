using System.Collections.Generic;
using System.Text;
using Awesome.Data.Sql.Builder.Delete;
using Awesome.Data.Sql.Builder.Insert;
using Awesome.Data.Sql.Builder.Select;
using Awesome.Data.Sql.Builder.Update;

namespace Awesome.Data.Sql.Builder.Renderers
{
    /// <summary>
    ///     An interface for rendering SQL in different syntaxes.
    /// </summary>
    public interface ISqlRenderer
    {
        /// <summary>
        /// Renders a LIMIT clause.
        /// </summary>
        /// <param name="limitClause">The limit clause.</param>
        /// <returns>The rendered clause</returns>
        string Limit(string limitClause);

        /// <summary>
        /// Renders a OFFSET clause.
        /// </summary>
        /// <param name="offsetClause">The offset clause.</param>
        /// <returns>The rendered clause</returns>
        string Offset(string offsetClause);

        /// <summary>
        /// Renders a LIMIT and OFFSET clause.
        /// </summary>
        /// <param name="limitClause">The limit clause.</param>
        /// <param name="offsetClause">The offset clause.</param>
        /// <returns>A rendered SQL string.</returns>
        string LimitOffset(string limitClause, string offsetClause);

        /// <summary>
        /// Renders columns for a select statement.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns>A rendered SQL string.</returns>
        string Columns(IEnumerable<string> columns);

        /// <summary>
        /// Renders the select.
        /// </summary>
        /// <param name="select">The select.</param>
        /// <returns>The rendered SQL SELECT clause.</returns>
        string RenderSelect(SelectStatement select);

        /// <summary>
        /// Renders the UPDATE statement.
        /// </summary>
        /// <param name="update">The update.</param>
        /// <returns>The rendered SQL UPDATE clause.</returns>
        string RenderUpdate(UpdateStatement update);

        /// <summary>
        /// Renders the INSERT statement.
        /// </summary>
        /// <param name="insert">The insert statement</param>
        /// <returns>The rendered SQL INSERT clause.</returns>
        string RenderInsert(InsertStatement insert);

        /// <summary>
        /// Renders the DELETE statement.
        /// </summary>
        /// <param name="delete">The delete statement</param>
        /// <returns>The rendered SQL DELETE clause.</returns>
        string RenderDelete(DeleteStatement delete);

        /// <summary>
        /// Appends the limit and offset clause of the select statement to the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
        void AppendLimitOffset(StringBuilder builder, SelectStatement select);

        /// <summary>
        /// Appends the select clause of the select statement to the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
        void AppendSelect(StringBuilder builder, SelectStatement select);

        /// <summary>
        /// Appends the group by clause of the select statement to the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
        void AppendGroupBy(StringBuilder builder, SelectStatement select);

        /// <summary>
        /// Appends the order by clause of the select statement to the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
        void AppendOrderBy(StringBuilder builder, SelectStatement select);

        /// <summary>
        /// Appends the from clause of the select statement to the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="statement">The statement.</param>
        void AppendFrom<T>(StringBuilder builder, SqlStatement<T> statement)
            where T : SqlStatement<T>;

        /// <summary>
        /// Appends the where clause of the select statement to the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="statement">The statement.</param>
        void AppendWhere<T>(StringBuilder builder, SqlStatement<T> statement)
            where T : SqlStatement<T>;
    }
}
