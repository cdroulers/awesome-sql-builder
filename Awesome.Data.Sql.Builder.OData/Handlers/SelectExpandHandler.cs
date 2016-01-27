using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.OData.Query;
using Awesome.Data.Sql.Builder.Select;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Awesome.Data.Sql.Builder.OData.Handlers
{
    internal static class SelectExpandHandler
    {
        public static void Handle<T>(ODataQueryOptions<T> queryOptions, SelectStatement statement)
        {
            if (queryOptions.SelectExpand == null)
            {
                return;
            }

            HandleItems(queryOptions.SelectExpand.SelectExpandClause, statement);
        }

        private static void HandleItems(SelectExpandClause selectAndExpand, SelectStatement statement)
        {
            foreach (var item in selectAndExpand.SelectedItems)
            {
                if (item is PathSelectItem)
                {
                    HandlePathSelectItem(statement, (PathSelectItem)item);
                }
                else if (item is ExpandedNavigationSelectItem)
                {
                    HandleExpandedNavigationSelectItem(statement, (ExpandedNavigationSelectItem)item);
                }
                else
                {
                    throw new NotSupportedException(string.Format("Selected item type '{0}' is not supported.", item.GetType().FullName));
                }
            }
        }

        private static void HandlePathSelectItem(SelectStatement statement, PathSelectItem item)
        {
            var property = item.SelectedPath.FirstSegment as PropertySegment;
            if (property != null)
            {
                statement.Columns(property.Property.Name);
            }
            else
            {
                throw new NotSupportedException(string.Format("PathSelectItem type '{0}' is not supported.", item.SelectedPath.FirstSegment.GetType().FullName));
            }
        }

        private static void HandleExpandedNavigationSelectItem(SelectStatement statement, ExpandedNavigationSelectItem item)
        {
            var property = item.PathToNavigationProperty.FirstSegment as NavigationPropertySegment;
            if (property != null)
            {
                var firstPart = property.NavigationProperty.Name;
                var newSelect = new SelectStatement(new List<string>());
                HandleItems(item.SelectAndExpand, newSelect);

                statement.Columns(newSelect.ColumnsList.Select(x => firstPart + "/" + x).ToArray());
            }
            else
            {
                throw new NotSupportedException(string.Format("ExpandedNavigationSelectItem type '{0}' is not supported.", item.PathToNavigationProperty.FirstSegment.GetType().FullName));
            }
        }
    }
}
