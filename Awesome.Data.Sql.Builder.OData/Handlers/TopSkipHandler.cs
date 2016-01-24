using System.Web.Http.OData.Query;
using Awesome.Data.Sql.Builder.Select;

namespace Awesome.Data.Sql.Builder.OData.Handlers
{
    internal static class TopSkipHandler
    {
        public static void Handle<T>(ODataQueryOptions<T> queryOptions, SelectStatement statement)
        {
            if (queryOptions.Top != null)
            {
                statement.Limit(queryOptions.Top.Value);
            }

            if (queryOptions.Skip != null)
            {
                statement.Offset(queryOptions.Skip.Value);
            }
        }
    }
}
