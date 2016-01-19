using System.Text;
using Awesome.Data.Sql.Builder.Renderers;

namespace Awesome.Data.Sql.Builder
{
    /// <summary>
    ///     Represents any part of a full SQL query.
    /// </summary>
    public interface ISqlFragment
    {
        /// <summary>
        /// Adds the SQL for the current object to the builder
        /// </summary>
        /// <param name="builder">The builder.</param>
        void BuildSql(StringBuilder builder);

        /// <summary>
        /// Adds the SQL for the current object to the builder
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="renderer">The SQL renderer to use.</param>
        void BuildSql(StringBuilder builder, ISqlRenderer renderer);
    }
}
