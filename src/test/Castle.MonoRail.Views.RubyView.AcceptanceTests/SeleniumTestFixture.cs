using Selenium;

namespace Castle.MonoRail.Views.RubyView.AcceptanceTests
{
	public abstract class SeleniumTestFixture
	{
		protected ISelenium Selenium { get; private set; }

		public virtual void SetUp()
		{
			Selenium = new DefaultSelenium("localhost", 4444, "*iehta", "http://localhost:2091");
			Selenium.Start();
			Selenium.SetSpeed("1");
		}

		public virtual void TearDown()
		{
			Selenium.Stop();
		}
	}
}