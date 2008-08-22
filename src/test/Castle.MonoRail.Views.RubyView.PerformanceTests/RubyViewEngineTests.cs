using System;
using System.IO;
using Castle.MonoRail.Framework;
using NUnit.Framework;
using Rhino.Mocks;
using rod.Commons.System.Diagnostics;

namespace Castle.MonoRail.Views.RubyView.PerformanceTests
{
	[TestFixture]
	public class RubyViewEngineTests : NUnitTestFixture
	{
		///<summary>
		/// 1000 = 65,5 sek
		///</summary>
		[Test]
		public void ProcessTest()
		{
			const int counter = 1000;
			IEngineContext engineContext;
			IController controller;
			IControllerContext controllerContext;
			RubyViewEngine engine;
			engineContext = MockRepository.GenerateMock<IEngineContext>();
			controller = MockRepository.GenerateMock<IController>();
			controllerContext = MockRepository.GenerateMock<IControllerContext>();

			var serviceProvider = MockRepository.GenerateMock<IServiceProvider>();
			var viewSourceLoader = new FileAssemblyViewSourceLoader(@"Views");
			serviceProvider.Expect(x => x.GetService(typeof(IViewSourceLoader))).Return(viewSourceLoader);
		
			var output = new StringWriterProxy();

			engine = new RubyViewEngine();
			engine.Service(serviceProvider);

			var timer = new HiPerfTimer();
			// pre
			engine.Process("simple\\outputexpression", output, engineContext, controller, controllerContext);

			timer.Start();
			for (int i = 0; i < counter; i++)
			{
				engine.Process("simple\\outputexpression", output, engineContext, controller, controllerContext);
			}
			timer.Stop();
			Console.Out.WriteLine("Time: {0}", timer);
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