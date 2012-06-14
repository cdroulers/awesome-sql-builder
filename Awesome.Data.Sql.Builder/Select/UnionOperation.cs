using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    /// <summary>
    ///     The SQL UNION operation
    /// </summary>
    public class UnionOperation : SetOperation<UnionOperation>
    {
        /// <summary>
        ///     If true, will perform a UNION ALL query instead of simple UNION
        /// </summary>
        public readonly bool All;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnionOperation"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="all">if set to <c>true</c> [all].</param>
        public UnionOperation(ISetQuery first, ISetQuery second, bool all = false)
            : base(first, second)
        {
            this.All = all;
        }

        protected override string SetOperator
        {
            get { return "UNION" + (this.All ? " ALL" : string.Empty); }
        }

        public override UnionOperation Clone()
        {
            return new UnionOperation((ISetQuery)this.First.Clone(), (ISetQuery)this.Second.Clone(), this.All);
        }
    }
}
