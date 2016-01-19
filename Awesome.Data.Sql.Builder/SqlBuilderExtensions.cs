using System.Text;
using Awesome.Data.Sql.Builder.Renderers;
using Awesome.Data.Sql.Builder.Select;

namespace Awesome.Data.Sql.Builder
{
    /// <summary>
    ///     Extensions for awesomeness!
    /// </summary>
    public static class SqlBuilderExtensions
    {
        /// <summary>
        /// Returns the statement as an SQL string.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="renderer">The renderer.</param>
        /// <returns>A string of SQL!</returns>
        public static string ToSql(this ISqlFragment self, ISqlRenderer renderer = null)
        {
            var builder = new StringBuilder();
            self.BuildSql(builder, renderer ?? new DefaultSqlRenderer());
            return builder.ToString().Trim();
        }

        /// <summary>
        /// Creates a UNION set query with both queries.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="with">The with.</param>
        /// <param name="all">if set to <c>true</c> [all].</param>
        /// <returns></returns>
        public static ISetQuery Union(this ISetQuery self, ISetQuery with, bool all = false)
        {
            return new UnionOperation(self, with, all);
        }

        /// <summary>
        /// Creates a INTERSECT set query with both queries.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="with">The with.</param>
        /// <param name="all">if set to <c>true</c> [all].</param>
        /// <returns></returns>
        public static ISetQuery Intersect(this ISetQuery self, ISetQuery with, bool all = false)
        {
            return new IntersectOperation(self, with, all);
        }

        /// <summary>
        /// Creates a EXCEPT set query with both queries.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="with">The with.</param>
        /// <param name="all">if set to <c>true</c> [all].</param>
        /// <returns></returns>
        public static ISetQuery Except(this ISetQuery self, ISetQuery with, bool all = false)
        {
            return new ExceptOperation(self, with, all);
        }

        /// <summary>
        /// Sets the alias for the specified from clause.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public static IFromClause As(this IFromClause self, string alias)
        {
            self.Alias = alias;
            return self;
        }

        /// <summary>
        /// Sets the alias for the specified set query.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public static ISetQuery As(this ISetQuery self, string alias)
        {
            self.Alias = alias;
            return self;
        }
    }
}
