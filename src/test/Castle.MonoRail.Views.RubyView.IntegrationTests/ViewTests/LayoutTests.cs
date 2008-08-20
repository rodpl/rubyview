using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;
using Rhino.Mocks;

namespace Castle.MonoRail.Views.RubyView.IntegrationTests.ViewTests
{
	[TestFixture]
	public class LayoutTests : ViewTestFixture
	{
		[TestCase("existingview", "Primary - Secondary - Existing View - Secondary - Primary", new[]{"Primary", "Secondary"})]
		[TestCase("putsString", "Primary - Secondary - Something to put:  - Secondary - Primary", new[] { "Primary", "Secondary" })]
		[TestCase("outputstring", "Primary - Secondary - RubyView reversed is weiVybuR - Secondary - Primary", new[] { "Primary", "Secondary" })]
		[TestCase("outputexpression", "Primary - Secondary - 2 + 2 is 4 - Secondary - Primary", new[] { "Primary", "Secondary" })]
		public void DifferentMultiplyLayout_Test(string viewName, string expected, string[] layoutNames)
		{
			_controllerContext.Expect(x => x.LayoutNames).Return(layoutNames);
			Assert.That(ProcessViewInEngine(@"simple\" + viewName), Is.EqualTo(expected));
		}

		[TestCase("existingview", "Primary - Primary - Existing View - Primary - Primary", new[] { "Primary", "Primary" })]
		[TestCase("putsString", "Primary - Primary - Something to put:  - Primary - Primary", new[] { "Primary", "Primary" })]
		[TestCase("outputstring", "Primary - Primary - RubyView reversed is weiVybuR - Primary - Primary", new[] { "Primary", "Primary" })]
		[TestCase("outputexpression", "Primary - Primary - 2 + 2 is 4 - Primary - Primary", new[] { "Primary", "Primary" })]
		public void SameMultiplyLayout_Test(string viewName, string expected, string[] layoutNames)
		{
			_controllerContext.Expect(x => x.LayoutNames).Return(layoutNames);
			Assert.That(ProcessViewInEngine(@"simple\" + viewName), Is.EqualTo(expected));
		}


		[Test]
		public void TestForTest()
		{
			var viewName = "outputexpression";
			var layoutNames = new[] { "Primary", "Primary" };
			var expected = "Primary - Primary - 2 + 2 is 4 - Primary - Primary";
			
			_controllerContext.Expect(x => x.LayoutNames).Return(layoutNames);
			Assert.That(ProcessViewInEngine(@"simple\" + viewName), Is.EqualTo(expected));
		}
	}
}