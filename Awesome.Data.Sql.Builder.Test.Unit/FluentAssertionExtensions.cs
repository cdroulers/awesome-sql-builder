using FluentAssertions;
using FluentAssertions.Primitives;

namespace Awesome.Data.Sql.Builder.Test.Unit
{
    public static class FluentAssertionExtensions
    {
        public static AndConstraint<StringAssertions> BeEquivalentToIgnoringNewLines(
            this StringAssertions self,
            string expected,
            string because = "",
            params string[] becauseArgs)
        {
            return self.Subject.Replace("\r\n", "\n")
                .Should()
                .BeEquivalentTo(
                    expected.Replace("\r\n", "\n"),
                    because,
                    becauseArgs);
        }
    }
}
