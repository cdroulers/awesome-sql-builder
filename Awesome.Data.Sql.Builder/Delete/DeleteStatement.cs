using System.Text;
using Awesome.Data.Sql.Builder.Renderers;

namespace Awesome.Data.Sql.Builder.Delete
{
    /// <summary>
    /// Represents an SQL DELETE statement.
    /// </summary>
    public class DeleteStatement : SqlStatement<DeleteStatement>
    {
        /// <summary>
        /// If this DELETE statement is on multiple tables, we can still only delete from one.
        /// </summary>
        public string TableToDelete { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteStatement"/> class.
        /// </summary>
        /// <param name="tableToDelete">The table to delete.</param>
        public DeleteStatement(string tableToDelete = null)
        {
            this.TableToDelete = tableToDelete;
        }

        private DeleteStatement(DeleteStatement statement)
            : base(statement)
        {
            this.TableToDelete = statement.TableToDelete;
        }

        /// <summary>
        /// Returns the statement as an SQL string.
        /// </summary>
        public override void BuildSql(StringBuilder builder, ISqlRenderer renderer)
        {
            builder.Append(renderer.RenderDelete(this));
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public override DeleteStatement Clone()
        {
            return new DeleteStatement(this);
        }
    }
}
