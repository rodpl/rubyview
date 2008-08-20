using System;
using System.IO;
using System.Runtime.Remoting;
using System.Text;
using Castle.MonoRail.Framework;
using Ruby;
using Ruby.Runtime;
using Microsoft.Scripting.Hosting;

namespace Castle.MonoRail.Views.RubyView
{
	public class RubyView
	{
		private TextWriter _output;
		private TextReader _input;

		public RubyView(TextReader input, TextWriter output)
		{
			_input = input;
			_output = output;
		}

		public void Render()
		{
			ScriptRuntime runtime = new ScriptRuntime();
			ScriptEngine rubyengine = IronRuby.GetEngine(runtime);
			RubyExecutionContext ctx = IronRuby.GetExecutionContext(runtime);

			ScriptScope scope = runtime.CreateScope();
			scope.SetVariable("output_buffer", _output);

			var template = new RubyTemplate(_input);
			template.AddRequire("mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			template.AddRequire("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			template.AddRequire("System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

			StringBuilder script = new StringBuilder();
			template.ToScript("render_page", script);
//			if (master != null)
//			{
//				master.Template.ToScript("render_layout", script);
//			}
//			else
//			{
				script.AppendLine(@"def render_layout
    yield
end");
//			}
//			script.AppendLine(@"def view_data.method_missing(methodname)
//    get_Item(methodname.to_s)
//end");
			script.AppendLine("render_layout { |content| render_page }");
			try
			{
				System.Diagnostics.Debug.WriteLine(script.ToString());
				ScriptSource source = rubyengine.CreateScriptSourceFromString(script.ToString());
				source.Execute(scope);
			}
			catch (Exception e)
			{
				throw new MonoRailException(script.ToString(), e);
			}
		}

		public class Fake
		{
			private readonly TextWriter _writer;

			public Fake(TextWriter writer)
			{
				_writer = writer;
			}

			public void Write(object target)
			{
				_writer.Write(target);
			}
		}
	}
}
