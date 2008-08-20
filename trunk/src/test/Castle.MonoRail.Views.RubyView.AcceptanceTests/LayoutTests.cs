using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;

namespace Castle.MonoRail.Views.RubyView.AcceptanceTests
{
	[TestFixture]
	public class LayoutTests : SeleniumTestFixture
	{
		[TestCase("existingview", "Primary - Existing View - Primary")]
		[TestCase("putsString", "Primary - Something to put: - Primary")]
		[TestCase("outputstring", "Primary - RubyView reversed is weiVybuR - Primary")]
		[TestCase("outputexpression", "Primary - 2 + 2 is 4 - Primary")]
		[TestCase("differentmultiplylayouts", "Primary - Secondary - RubyView reversed is weiVybuR - Secondary - Primary")]
		[TestCase("samemultiplylayouts", "Primary - Primary - RubyView reversed is weiVybuR - Primary - Primary")]
		public void Simple_Test(string viewName, string expected)
		{
			Selenium.Open(string.Format("layout/{0}.aspx", viewName));
			Selenium.WaitForPageToLoad("3000");
			Assert.That(Selenium.GetBodyText(), Is.EqualTo(expected.Trim()));
		}
	}
}