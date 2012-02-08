using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder
{
    public interface ISqlStatement<T>
        where T : ISqlStatement<T>
    {
        T From(params string[] tables);
        T Where(string condition, bool or = false);

        string ToSql();
    }
}
