using System;

namespace Awesome.Data.Sql.Builder.Renderers
{
    /// <summary>
    ///     An SQL Server SQL renderer.
    /// </summary>
    public class SqlServer2012SqlRenderer : SqlServerSqlRenderer
    {
        /// <summary>
        /// Limits the specified limit clause.
        /// </summary>
        /// <param name="limitClause">The limit clause.</param>
        /// <returns>A rendered SQL string.</returns>
        public override string Limit(string limitClause)
        {
            return string.Format("FETCH NEXT {0} ROWS ONLY", limitClause);
        }

        /// <summary>
        /// Offsets the specified offset clause.
        /// </summary>
        /// <param name="offsetClause">The offset clause.</param>
        /// <returns>A rendered SQL string.</returns>
        public override string Offset(string offsetClause)
        {
            return string.Format("OFFSET {0} ROWS", offsetClause);
        }

        /// <summary>
        /// Renders a LIMIT and OFFSET clause.
        /// </summary>
        /// <param name="limitClause">The limit clause.</param>
        /// <param name="offsetClause">The offset clause.</param>
        /// <returns> A rendered SQL string. </returns>
        public override string LimitOffset(string limitClause, string offsetClause)
        {
            return this.Offset(offsetClause) + Environment.NewLine + this.Limit(limitClause);
        }
    }
}
