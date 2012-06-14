using System.Text;

namespace System.Data.Sql.Builder
{
    /// <summary>
    ///     Interface for from clauses.
    /// </summary>
    public interface IFromClause : ICloneable, ISqlFragment
    {
        /// <summary>
        /// Adds the SQL for the current object to the builder
        /// </summary>
        /// <param name="builder">The builder.</param>
        void BuildFromSql(StringBuilder builder);

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        IFromClause CloneFrom();

        /// <summary>
        /// Gets the alias of the from clause.
        /// </summary>
        string Alias { get; set; }
    }
}