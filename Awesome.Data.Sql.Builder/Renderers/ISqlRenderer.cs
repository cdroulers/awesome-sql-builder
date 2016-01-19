using System.Collections.Generic;
using System.Text;
using Awesome.Data.Sql.Builder.Select;

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
        /// <param name="select">The select.</param>
        void AppendFrom(StringBuilder builder, SelectStatement select);

        /// <summary>
        /// Appends the where clause of the select statement to the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="select">The select.</param>
        void AppendWhere(StringBuilder builder, SelectStatement select);
    }
}
