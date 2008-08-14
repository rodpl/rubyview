using System;
using System.Collections.Generic;
using System.IO;
using Castle.MonoRail.Framework;

namespace Castle.MonoRail.Views.RubyView
{
	public class RubyViewEngine : ViewEngineBase
	{
		public override object CreateJSGenerator(JSCodeGeneratorInfo generatorInfo, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		public override void GenerateJS(string templateName, TextWriter output, JSCodeGeneratorInfo generatorInfo, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		public override void Process(string templateName, TextWriter output, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		public override void Process(string templateName, string layoutName, TextWriter output, IDictionary<string, object> parameters)
		{
			throw new NotImplementedException();
		}

		public override void RenderStaticWithinLayout(string contents, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		public override void ProcessPartial(string partialName, TextWriter output, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		public override bool SupportsJSGeneration
		{
			get { throw new NotImplementedException(); }
		}

		public override string JSGeneratorFileExtension
		{
			get { throw new NotImplementedException(); }
		}

		public override string ViewFileExtension
		{
			get { throw new NotImplementedException(); }
		}
	}
}