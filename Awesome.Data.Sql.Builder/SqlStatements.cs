using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
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
        /// <returns></returns>
        public static SelectStatement Select(params string[] columns)
        {
            return new SelectStatement(columns);
        }
    }
}
