using NUnit.Framework.Constraints;

namespace Awesome.Data.Sql.Builder.Test.Unit.Contraints
{
    /// <summary>
    ///     Required because tests run on linux and line endings aren't the same as the code which is in Windows format.
    /// </summary>
    public class SqlCompareConstraint : EqualConstraint
    {
        public SqlCompareConstraint(object expected)
            : base(CleanString(expected))
        {
        }

        private static object CleanString(object value)
        {
            if (value is string)
            {
                value = ((string)value).Replace("\r\n", "\n");
            }

            return value;
        }

        public override bool Matches(object actualValue)
        {
            return base.Matches(CleanString(actualValue));
        }

        public static SqlCompareConstraint EqualTo(object expected)
        {
            return new SqlCompareConstraint(expected);
        }
    }
}
