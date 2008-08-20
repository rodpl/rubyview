using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using Castle.MonoRail.Framework;
using Microsoft.Scripting.Hosting;
using Ruby;
[assembly: AllowPartiallyTrustedCallers()]
namespace Castle.MonoRail.Views.RubyView
{
	public class RubyViewEngine : ViewEngineBase
	{
		private const string VIEW_FILE_EXTENSION = ".erb";

		/// <summary>
		/// Implementors should return a generator instance if
		/// the view engine supports JS generation.
		/// </summary>
		/// <param name="generatorInfo">The generator info.</param>
		/// <param name="context">The request context.</param>
		/// <param name="controller">The controller.</param>
		/// <param name="controllerContext">The controller context.</param>
		/// <returns>A JS generator instance</returns>
		public override object CreateJSGenerator(JSCodeGeneratorInfo generatorInfo, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Processes the js generation view template - using the templateName
		/// to obtain the correct template, and using the specified <see cref="T:System.IO.TextWriter"/>
		/// to output the result.
		/// </summary>
		/// <param name="templateName">Name of the template.</param>
		/// <param name="output">The output.</param>
		/// <param name="generatorInfo">The generator info.</param>
		/// <param name="context">The request context.</param>
		/// <param name="controller">The controller.</param>
		/// <param name="controllerContext">The controller context.</param>
		public override void GenerateJS(string templateName, TextWriter output, JSCodeGeneratorInfo generatorInfo, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Processes the view - using the templateName
		/// to obtain the correct template
		/// and writes the results to the System.IO.TextWriter.
		/// </summary>
		/// <param name="templateName"></param>
		/// <param name="output"></param>
		/// <param name="context"></param>
		/// <param name="controller"></param>
		/// <param name="controllerContext"></param>
		public override void Process(string templateName, TextWriter output, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			var fileName = string.Concat(templateName, ViewFileExtension);

			var viewSource = ViewSourceLoader.GetViewSource(fileName);
			if(viewSource == null)
				throw new MonoRailException(404, "No such view", string.Format("Missing or invalid view: {0}", fileName));

			var scriptRuntime = IronRuby.CreateRuntime();
			var scriptScope = scriptRuntime.CreateScope();
			var scriptEngine = IronRuby.GetEngine(scriptRuntime);

			var view = new RubyView(new StreamReader(viewSource.OpenViewStream()), output);
			view.Render();
		}

		/// <summary>
		/// Processes the view - using the templateName
		/// to obtain the correct template
		/// and writes the results to the <see cref="T:System.IO.TextWriter"/>.
		/// </summary>
		/// <param name="templateName"></param>
		/// <param name="layoutName"></param>
		/// <param name="output"></param>
		/// <param name="parameters"></param>
		public override void Process(string templateName, string layoutName, TextWriter output, IDictionary<string, object> parameters)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Wraps the specified content in the layout using the
		/// context to output the result.
		/// </summary>
		/// <param name="contents"></param>
		/// <param name="context"></param>
		/// <param name="controller"></param>
		/// <param name="controllerContext"></param>
		public override void RenderStaticWithinLayout(string contents, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Should process the specified partial. The partial name must contains
		/// the path relative to the views folder.
		/// </summary>
		/// <param name="partialName">The partial name.</param>
		/// <param name="output">The output.</param>
		/// <param name="context">The request context.</param>
		/// <param name="controller">The controller.</param>
		/// <param name="controllerContext">The controller context.</param>
		public override void ProcessPartial(string partialName, TextWriter output, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a value indicating whether the view engine
		/// support the generation of JS.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if JS generation is supported; otherwise, <c>false</c>.
		/// </value>
		public override bool SupportsJSGeneration
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the JS generator file extension.
		/// </summary>
		/// <value>The JS generator file extension.</value>
		public override string JSGeneratorFileExtension
		{
			get { return null; }
		}

		/// <summary>
		/// Gets the view file extension.
		/// </summary>
		/// <value>The view file extension.</value>
		public override string ViewFileExtension
		{
			get { return VIEW_FILE_EXTENSION; }
		}
	}
}