using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;

namespace Castle.MonoRail.Views.RubyView.IntegrationTests.ViewTests
{
	[TestFixture]
	public class BasicFunctionalityTests : ViewTestFixture
	{
		[Test]
		public void SimpleProcessView()
		{
			const string expected = "Welcome to the RubyView world.";
			var output = ProcessViewInEngine(@"home\index");

			Assert.That(output, Is.EqualTo(expected));
		}
	}
}