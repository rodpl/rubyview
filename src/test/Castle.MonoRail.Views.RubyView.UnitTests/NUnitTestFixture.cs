using NUnit.Framework;

namespace Castle.MonoRail.Views.RubyView.UnitTests
{
	[Category("Unit")]
	public abstract class NUnitTestFixture
	{
		[SetUp]
		public virtual void SetUp()
		{
		}

		[TearDown]
		public virtual void TearDown()
		{
		}
	}
}