using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Castle.MonoRail.Framework;
using Castle.MonoRail.Framework.Helpers;
using Castle.MonoRail.Framework.Services;
using Castle.MonoRail.Framework.Test;
using Castle.MonoRail.Framework.Views.NVelocity;
using Castle.MonoRail.Views.AspView;
using Castle.MonoRail.Views.Brail;
using NUnit.Framework;
using Rhino.Mocks;

namespace Castle.MonoRail.Views.RubyView.PerformanceTests
{
	[TestFixture]
	public class RubyViewEngineTests : NUnitTestFixture
	{
		private IEngineContext _engineContext;
		private IController _controller;
		private IControllerContext _controllerContext;
		private IServiceProvider _serviceProvider;
		private FileAssemblyViewSourceLoader _viewSourceLoader;
		protected HelperDictionary Helpers;
		protected DefaultViewComponentFactory ViewComponentFactory;
		private string viewSourcePath;

		public override void SetUp()
		{
			base.SetUp();
//
//			string Area = null;
//			string ControllerName = "test_controller";
//			string Action = "test_action";
//
//			var services = new StubMonoRailServices();
//			services.UrlBuilder = new DefaultUrlBuilder(new StubServerUtility(), new StubRoutingEngine());
//			services.UrlTokenizer = new DefaultUrlTokenizer();
//			UrlInfo urlInfo = new UrlInfo(
//				"example.org", "test", "/TestBrail", "http", 80,
//				"http://test.example.org/test_area/test_controller/test_action.tdd",
//				Area, ControllerName, Action, "tdd", "no.idea");
//			_engineContext = new StubEngineContext(new StubRequest(), new StubResponse(), services, urlInfo);
//			_engineContext.AddService(typeof(IUrlBuilder), services.UrlBuilder);
//			_engineContext.AddService(typeof(IUrlTokenizer), services.UrlTokenizer);
//
//
//			_engineContext.AddService<IViewComponentFactory>(ViewComponentFactory);
//			_controllerContext= new ControllerContext();
//			_controllerContext.Helpers = Helpers;
//			_controllerContext.PropertyBag = PropertyBag;
//			StubEngineContext.CurrentControllerContext = _controllerContext;
//
//
//			ViewComponentFactory = new DefaultViewComponentFactory();
//			ViewComponentFactory.Service(StubEngineContext);
//			ViewComponentFactory.Initialize();
//
//			Helpers = new HelperDictionary();
//			Helpers["urlhelper"] = Helpers["url"] = new UrlHelper(_engineContext);
//			Helpers["htmlhelper"] = Helpers["html"] = new HtmlHelper(_engineContext);
//			Helpers["dicthelper"] = Helpers["dict"] = new DictHelper(_engineContext);
//			Helpers["DateFormatHelper"] = Helpers["DateFormat"] = new DateFormatHelper(_engineContext);
//
//			string viewPath = Path.Combine(viewSourcePath, "Views");
//
//			FileAssemblyViewSourceLoader loader = new FileAssemblyViewSourceLoader(viewPath);

			_controller = MockRepository.GenerateMock<IController>();
			_controllerContext = MockRepository.GenerateMock<IControllerContext>();
			_serviceProvider = MockRepository.GenerateMock<IServiceProvider>();
			_viewSourceLoader = new FileAssemblyViewSourceLoader(@"Views");
			_serviceProvider.Expect(x => x.GetService(typeof(IViewSourceLoader))).Return(_viewSourceLoader);
		}

		///<summary>
		/// 1000 = ~65,5 sek
		/// 1000 = ~47,32 sek
		///</summary>
		[Test]
		public void ProcessTest()
		{
			const int counter = 10;
			var templateName = "simple\\outputexpression";
			var stopwatch = new Stopwatch();
			var output = new StringWriterProxy();

			var rubyViewEngine = new RubyViewEngine();
			rubyViewEngine.Service(_serviceProvider);
			rubyViewEngine.Process(templateName, output, _engineContext, _controller, _controllerContext);
			stopwatch.Start();
			for (int i = 0; i < counter; i++)
			{
				output = new StringWriterProxy();
				rubyViewEngine.Process(templateName, output, _engineContext, _controller, _controllerContext);
			}
			stopwatch.Stop();
			var rubyViewEngineTime = (double) stopwatch.ElapsedMilliseconds/1000;

//			var brailViewEngine = GetBrailEngine();
//			brailViewEngine.Service(_serviceProvider);
//			brailViewEngine.Process(templateName, output, _engineContext, _controller, _controllerContext);
//			stopwatch.Start();
//			for (int i = 0; i < counter; i++)
//			{
//				brailViewEngine.Process(templateName, output, _engineContext, _controller, _controllerContext);
//			}
//			stopwatch.Stop();
//			var brailViewEngineTime = (double)stopwatch.ElapsedMilliseconds / 1000;



			Console.Out.WriteLine("RubyView: {0}", rubyViewEngineTime);
			//Console.Out.WriteLine("BrailView: {0}", brailViewEngineTime);
		}

		private ViewEngineBase GetBrailEngine()
		{
			var engine = new BooViewEngine();
			engine.Options = new BooViewEngineOptions();
			engine.Options.SaveDirectory = Environment.CurrentDirectory;
			engine.Options.SaveToDisk = false;
			engine.Options.Debug = true;
			engine.Options.BatchCompile = false;

			engine.SetViewSourceLoader(_viewSourceLoader);
			engine.Initialize();
			return engine;
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