using NUnit.Framework;
namespace Castle.MonoRail.Views.RubyView.AcceptanceTests
{
	[Category("Acceptance")]
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