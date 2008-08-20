using Selenium;

namespace Castle.MonoRail.Views.RubyView.AcceptanceTests
{
	public abstract class SeleniumTestFixture : NUnitTestFixture
	{
		protected ISelenium Selenium { get; private set; }

		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			Selenium = new DefaultSelenium("localhost", 4444, "*iehta", "http://localhost:2091");
			Selenium.Start();
		}

		public override void TestFixtureTearDown()
		{
			base.TestFixtureTearDown();
			Selenium.Stop();
		}
	}
}