using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
{
    /// <summary>
    ///     Extensions for awesomeness!
    /// </summary>
    public static class SqlBuilderExtensions
    {
        /// <summary>
        ///     Returns the statement as an SQL string.
        /// </summary>
        /// <returns></returns>
        public static string ToSql(this ISqlFragment self)
        {
            var builder = new StringBuilder();
            self.BuildSql(builder);
            return builder.ToString().Trim();
        }
    }
}
