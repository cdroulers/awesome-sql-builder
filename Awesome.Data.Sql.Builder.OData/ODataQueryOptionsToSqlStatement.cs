using System.Collections.Generic;
using System.Web.Http.OData.Query;
using Awesome.Data.Sql.Builder.OData.Handlers;
using Awesome.Data.Sql.Builder.Select;

namespace Awesome.Data.Sql.Builder.OData
{
    /// <summary>
    /// A mapper class that transforms <see cref="System.Web.Http.OData.Query.ODataQueryOptions"/> into a <see cref="Awesome.Data.Sql.Builder.Select.SelectStatement"/>.
    /// 
    /// Currently supports `$select`, `$top`, `$skip` and `$inlinecount=allpages`.
    /// </summary>
    public class ODataQueryOptionsToSqlStatement
    {
        /// <summary>
        /// Transforms the options into a SELECT statement.
        /// </summary>
        /// <typeparam name="T">The type of ODataQueryOptions.</typeparam>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>A Select statement with columns and limit offset.</returns>
        public IList<SelectStatement> ToSelect<T>(ODataQueryOptions<T> queryOptions)
        {
            var results = new List<SelectStatement>();
            var select = new SelectStatement(new List<string>());

            SelectExpandHandler.Handle(queryOptions, select);
            TopSkipHandler.Handle(queryOptions, select);

            results.Add(select);

            if (queryOptions.InlineCount != null && queryOptions.InlineCount.Value == InlineCountValue.AllPages)
            {
                results.Add(select.ToCount());
            }

            return results;
        }
    }
}
