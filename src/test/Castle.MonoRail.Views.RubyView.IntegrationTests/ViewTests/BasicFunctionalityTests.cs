using System.IO;
using Castle.MonoRail.Framework;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;
using Rhino.Mocks;

namespace Castle.MonoRail.Views.RubyView.IntegrationTests.ViewTests
{
	[TestFixture]
	public class BasicFunctionalityTests
	{
		[SetUp]
		public void SetUp()
		{

		}

		[Test]
		public void SimpleProcessView()
		{
			var engine = new RubyViewEngine();
			var output = new StringWriter();
			var engineContext = MockRepository.GenerateMock<IEngineContext>();
			var controller = MockRepository.GenerateMock<IController>();
			var controllerContext = MockRepository.GenerateMock<IControllerContext>();
			engine.Process(@"home\index", output, engineContext, controller, controllerContext);
			Assert.That(output, Is.EqualTo("Welcome to the RubyView world."));
		}
	}
}