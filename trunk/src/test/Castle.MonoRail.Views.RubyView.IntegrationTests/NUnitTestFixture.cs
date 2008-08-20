using NUnit.Framework;
namespace Castle.MonoRail.Views.RubyView.IntegrationTests
{
	[Category("Integration")]
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