using System;

namespace Awesome.Data.Sql.Builder
{
    /// <summary>
    ///     Interface for from clauses.
    /// </summary>
    public interface IFromClause : ICloneable, ISqlFragment
    {
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        IFromClause CloneFrom();

        /// <summary>
        /// Gets the alias of the from clause.
        /// </summary>
        string Alias { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is complex and requires wrapping in a select statement.
        /// </summary>
        bool IsComplex { get; }
    }
}
