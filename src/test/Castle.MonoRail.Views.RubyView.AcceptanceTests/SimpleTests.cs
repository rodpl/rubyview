using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;
using Selenium;

namespace Castle.MonoRail.Views.RubyView.AcceptanceTests
{
	[TestFixture]
	public class SimpleTests : SeleniumTestFixture
	{
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
		}

		[TearDown]
		public override void TearDown()
		{
			base.TearDown();
		}

		[Test]
		public void ExistingView_Test()
		{
			Selenium.Open("simple/existingview.aspx");
			Selenium.WaitForPageToLoad("3000");
			Assert.That(Selenium.GetBodyText(), Is.EqualTo("Existing View"));
		}
	}
}