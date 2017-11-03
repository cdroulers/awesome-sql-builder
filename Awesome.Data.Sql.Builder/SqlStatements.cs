using Awesome.Data.Sql.Builder.Insert;
using Awesome.Data.Sql.Builder.Select;
using Awesome.Data.Sql.Builder.Update;

namespace Awesome.Data.Sql.Builder
{
    /// <summary>
    ///     Utility class to create statements without constructor
    /// </summary>
    public static class SqlStatements
    {
        /// <summary>
        /// Creates a SELECT statement.
        /// </summary>
        /// <param name="columns">The columns to select.</param>
        /// <returns>A <see cref="SelectStatement"/> instance</returns>
        public static SelectStatement Select(params string[] columns)
        {
            return new SelectStatement(columns);
        }

        /// <summary>
        /// Creates an INSERT statement.
        /// </summary>
        /// <returns>A <see cref="InsertStatement"/> instance</returns>
        public static InsertStatement Insert()
        {
            return new InsertStatement();
        }

        /// <summary>
        /// Creates an UPDATE statement from a columns list.
        /// </summary>
        /// <param name="columns">The columns to update.</param>
        /// <returns>A <see cref="UpdateStatement"/> instance</returns>
        public static UpdateStatement Update(params string[] columns)
        {
            return new UpdateStatement(columns);
        }
    }
}
