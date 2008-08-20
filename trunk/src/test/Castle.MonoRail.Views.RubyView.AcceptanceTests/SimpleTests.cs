using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;

namespace Castle.MonoRail.Views.RubyView.AcceptanceTests
{
	[TestFixture]
	public class SimpleTests : SeleniumTestFixture
	{
		[TestCase("existingview", "Existing View")]
		[TestCase("putsString", "Something to put: ")]
		[TestCase("outputstring", "RubyView reversed is weiVybuR")]
		[TestCase("outputexpression", "2 + 2 is 4")]
		public void Simple_Test(string viewName, string expected)
		{
			Selenium.Open(string.Format("simple/{0}.aspx", viewName));
			Selenium.WaitForPageToLoad("3000");
			Assert.That(Selenium.GetBodyText(), Is.EqualTo(expected.Trim()));
		}
	}
}