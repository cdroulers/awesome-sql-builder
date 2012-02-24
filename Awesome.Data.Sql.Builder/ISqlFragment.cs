using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
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
    }
}
