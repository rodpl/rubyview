using Selenium;

namespace Castle.MonoRail.Views.RubyView.AcceptanceTests
{
	public abstract class SeleniumTestFixture : NUnitTestFixture
	{
		protected ISelenium Selenium { get; private set; }

		public override void SetUp()
		{
			base.SetUp();
			Selenium = new DefaultSelenium("localhost", 4444, "*iehta", "http://localhost:2091");
			Selenium.Start();
			Selenium.SetSpeed("1");
		}

		public override void TearDown()
		{
			base.SetUp();
			Selenium.Stop();
		}
	}
}