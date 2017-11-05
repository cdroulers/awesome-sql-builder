using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Awesome.Data.Sql.Builder.Renderers;

namespace Awesome.Data.Sql.Builder.Update
{
    /// <summary>
    /// Represents an SQL UPDATE statement.
    /// </summary>
    public class UpdateStatement : SqlStatement<UpdateStatement>
    {
        private readonly List<string> columnsList;

        /// <summary>
        /// If this UPDATE statement is on multiple tables, we can still only update one.
        /// </summary>
        public string TableToUpdate { get; private set; }

        /// <summary>
        /// Returns all columns
        /// </summary>
        public IReadOnlyCollection<string> ColumnsList
        {
            get { return new ReadOnlyCollection<string>(this.columnsList); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateStatement"/> class.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="tableToUpdate">The table to update.</param>
        public UpdateStatement(IEnumerable<string> columns, string tableToUpdate = null)
        {
            this.columnsList = new List<string>(columns);
            this.TableToUpdate = tableToUpdate;
        }

        private UpdateStatement(UpdateStatement statement)
            : base(statement)
        {
            this.columnsList = statement.columnsList.ToList();
            this.TableToUpdate = statement.TableToUpdate;
        }

        /// <summary>
        /// Adds columns to SELECT
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public UpdateStatement Columns(params string[] columns)
        {
            return this.Columns(false, columns);
        }

        /// <summary>
        /// Adds columns to SELECT. Clears the current one if specified.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="clearCurrent">if set to <c>true</c> [clear current].</param>
        /// <returns></returns>
        public UpdateStatement Columns(bool clearCurrent, params string[] columns)
        {
            if (clearCurrent)
            {
                this.columnsList.Clear();
            }

            this.columnsList.AddRange(columns);
            return this;
        }

        /// <summary>
        /// Returns the statement as an SQL string.
        /// </summary>
        public override void BuildSql(StringBuilder builder, ISqlRenderer renderer)
        {
            builder.Append(renderer.RenderUpdate(this));
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public override UpdateStatement Clone()
        {
            return new UpdateStatement(this);
        }
    }
}
