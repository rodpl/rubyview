using System;
using System.IO;
using System.Text;
using System.Web;
using Castle.MonoRail.Framework;
using Rhino.Mocks;

namespace Castle.MonoRail.Views.RubyView.IntegrationTests.ViewTests
{
	public class ViewTestFixture : NUnitTestFixture
	{
		protected IEngineContext _engineContext;
		protected IController _controller;
		protected IControllerContext _controllerContext;
		protected RubyViewEngine _engine;

		
		public override void SetUp()
		{
			base.SetUp();
			_engineContext = MockRepository.GenerateMock<IEngineContext>();
			_controller = MockRepository.GenerateMock<IController>();
			_controllerContext = MockRepository.GenerateMock<IControllerContext>();

			var serviceProvider = MockRepository.GenerateMock<IServiceProvider>();
			var viewSourceLoader = new FileAssemblyViewSourceLoader(@"ViewTests\Views");
			serviceProvider.Expect(x => x.GetService(typeof(IViewSourceLoader))).Return(viewSourceLoader);
			_engine = new RubyViewEngine();
			_engine.Service(serviceProvider);

		}

		protected string ProcessViewInEngine(string viewName)
		{
			// BUG: Issue this, fix this and then test it.
			var output = new StringWriterProxy();
			_engine.Process(viewName, output, _engineContext, _controller, _controllerContext);
			return output.ToString();
		}

		/// <summary>
		/// This is b u g solution
		/// System.ArgumentException: wrong number or type of arguments for `write'
		/// w _stub_$13##13(Closure , CallSite , RubyMethodScope , Object , Int32 )
		/// w _stub_MatchCaller(DynamicSiteTarget`4 , CallSite , Object[] )
		/// 
		/// It looks that Microsoft.Scripting looks for methods defined exactly in object, not inherited.
		/// </summary>
		public class StringWriterProxy : StringWriter
		{
			public override void Write(object value)
			{
				base.Write(value);
			}
		}
	}
}