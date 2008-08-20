using NUnit.Framework;
namespace Castle.MonoRail.Views.RubyView.AcceptanceTests
{
	[Category("Acceptance")]
	public abstract class NUnitTestFixture
	{
		[TestFixtureSetUp]
		public virtual void TestFixtureSetUp()
		{
		}

		[TestFixtureTearDown]
		public virtual void TestFixtureTearDown()
		{
		}

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