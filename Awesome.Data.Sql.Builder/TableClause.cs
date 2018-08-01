using System.Text;
using Awesome.Data.Sql.Builder.Renderers;

namespace Awesome.Data.Sql.Builder
{
    /// <summary>
    ///     A simple table clause.
    /// </summary>
    public class TableClause : FromClause
    {
        private readonly string name;

        /// <summary>
        ///     The table name.
        /// </summary>
        public string Name => this.name;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableClause"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        public TableClause(string name, string alias = null)
        {
            this.name = name;
            this.Alias = alias;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public TableClause Clone()
        {
            return new TableClause(this.Name);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public override IFromClause CloneFrom()
        {
            return this.Clone();
        }

        /// <summary>
        /// Returns the FROM clause as SQL.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="renderer">The renderer.</param>
        public override void BuildSql(StringBuilder builder, ISqlRenderer renderer)
        {
            builder.Append(this.Name + (!string.IsNullOrWhiteSpace(this.Alias) ? " " + this.Alias : string.Empty));
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>True if they are equal.</returns>
        public bool Equals(TableClause other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.Name, this.Name);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(TableClause))
            {
                return false;
            }

            return this.Equals((TableClause)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Name != null ? this.Name.GetHashCode() : 0;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Name: {0}", this.Name);
        }
    }
}
