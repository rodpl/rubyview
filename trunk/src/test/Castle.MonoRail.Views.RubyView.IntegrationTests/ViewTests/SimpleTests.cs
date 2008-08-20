using Castle.MonoRail.Framework;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;

namespace Castle.MonoRail.Views.RubyView.IntegrationTests.ViewTests
{
	[TestFixture]
	public class SimpleTests : ViewTestFixture
	{

		[Test]
		public void NonExistingView_Test()
		{
			Assert.That(() => ProcessViewInEngine(@"simple\nonexistingview"), Throws.Exception<MonoRailException>(Has.Property("Message").Contains("Missing or invalid view")));
		}

		[TestCase("existingview", "Existing View")]
		[TestCase("putsString", "Something to put: ")]
		[TestCase("outputstring", "RubyView reversed is weiVybuR")]
		[TestCase("outputexpression", "2 + 2 is 4")]
		public void Simple_Test(string viewName, string expected)
		{
			Assert.That(ProcessViewInEngine(@"simple\" + viewName), Is.EqualTo(expected));
		}

//		[Test]
//		public void TestForTest()
//		{
//			var viewName = "outputexpression";
//			var expected = "2 + 2 is 4";
//			Assert.That(ProcessViewInEngine(@"simple\" + viewName), Is.EqualTo(expected));
//		}
	}
}