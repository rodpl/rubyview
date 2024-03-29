using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Castle.MonoRail.Framework;
using Microsoft.Scripting.Hosting;
using Ruby;

namespace Castle.MonoRail.Views.RubyView
{
	public class RubyViewEngine : ViewEngineBase
	{
		private readonly ScriptRuntime _scriptRuntime;
		private const string VIEW_FILE_EXTENSION = ".erb";

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

		public RubyViewEngine()
		{
			_scriptRuntime = IronRuby.CreateRuntime();
		}

		/// <summary>
		/// Implementors should return a generator instance if
		/// the view engine supports JS generation.
		/// </summary>
		/// <param name="generatorInfo">The generator info.</param>
		/// <param name="context">The request context.</param>
		/// <param name="controller">The controller.</param>
		/// <param name="controllerContext">The controller context.</param>
		/// <returns>A JS generator instance</returns>
		public override object CreateJSGenerator(JSCodeGeneratorInfo generatorInfo, IEngineContext context,
		                                         IController controller, IControllerContext controllerContext)
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
		public override void GenerateJS(string templateName, TextWriter output, JSCodeGeneratorInfo generatorInfo,
		                                IEngineContext context, IController controller, IControllerContext controllerContext)
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
		/// <exception cref="MonoRailException"><c>MonoRailException</c>.</exception>
		public override void Process(string templateName, TextWriter output, IEngineContext context, IController controller,
		                             IControllerContext controllerContext)
		{
			var viewFileName = string.Concat(templateName, ViewFileExtension);

			var viewSource = ViewSourceLoader.GetViewSource(viewFileName);
			if (viewSource == null)
				throw new MonoRailException(404, "No such view", string.Format("Missing or invalid view: {0}", viewFileName));
			var layoutNames = controllerContext.LayoutNames;

			var view = new RubyView(viewFileName, output, viewSource, _scriptRuntime);
			if (layoutNames != null)
			{
				var last = view;

				for (int i = layoutNames.Length - 1; i >= 0; i--)
				{
					Debug.Write(i);
					IViewSource layoutSource;
					var layoutFileName = string.Concat("Layouts\\", layoutNames[i].Trim(), ViewFileExtension);
					layoutSource = ViewSourceLoader.GetViewSource(layoutFileName);
					if (layoutSource == null)
						throw new MonoRailException(string.Format("Layout '{0}' cannot be found or loaded.", layoutNames[i].Trim()));
					last.Parent = new RubyView(layoutFileName, output, layoutSource, _scriptRuntime);
					last = last.Parent;
				}
			}
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
		public override void Process(string templateName, string layoutName, TextWriter output,
		                             IDictionary<string, object> parameters)
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
		public override void RenderStaticWithinLayout(string contents, IEngineContext context, IController controller,
		                                              IControllerContext controllerContext)
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
		public override void ProcessPartial(string partialName, TextWriter output, IEngineContext context,
		                                    IController controller, IControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}
	}
}