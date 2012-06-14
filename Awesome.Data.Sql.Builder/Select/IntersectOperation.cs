using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Sql.Builder.Select
{
    /// <summary>
    ///     The SQL INTERSECT operation
    /// </summary>
    public class IntersectOperation : SetOperation<IntersectOperation>
    {
        /// <summary>
        ///     If true, will perform a INTERSECT ALL query instead of simple INTERSECT
        /// </summary>
        public readonly bool All;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntersectOperation"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="all">if set to <c>true</c> [all].</param>
        public IntersectOperation(ISetQuery first, ISetQuery second, bool all = false)
            : base(first, second)
        {
            this.All = all;
        }

        protected override string SetOperator
        {
            get { return "INTERSECT" + (this.All ? " ALL" : string.Empty); }
        }

        public override IntersectOperation Clone()
        {
            return new IntersectOperation((ISetQuery)this.First.Clone(), (ISetQuery)this.Second.Clone());
        }
    }
}
