using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Castle.MonoRail.Framework;
using Microsoft.Scripting.Hosting;
using Ruby;
using Ruby.Runtime;

namespace Castle.MonoRail.Views.RubyView
{
	public class RubyView
	{
		private readonly string _fileName;
		private readonly TextWriter _output;
		private TextReader _input;
		private RubyTemplate _template;

		#region constructors ...

		public RubyView(string fileName, TextWriter output, IViewSource viewSource)
		{
			ViewSource = viewSource;
			_fileName = fileName;
			_output = output;
			_input = new StreamReader(ViewSource.OpenViewStream());
		}

		~RubyView()
		{
			Debug.WriteLine(String.Format("{0} : ~RubyView- {1}", GetType(), "START"));
		}

		#endregion

		public IViewSource ViewSource { get; set; }

		public string FileName
		{
			get { return _fileName; }
		}

		public RubyView Parent { get; set; }

		public RubyTemplate Template
		{
			get
			{
				if (_template == null)
					_template = new RubyTemplate(_input);
				return _template;
			}
			set { _template = value; }
		}

		public void Render()
		{
			ScriptRuntime runtime = new ScriptRuntime();
			ScriptEngine rubyengine = IronRuby.GetEngine(runtime);
			RubyExecutionContext ctx = IronRuby.GetExecutionContext(runtime);

			ScriptScope scope = runtime.CreateScope();
			scope.SetVariable("output_buffer", _output);

			Template = new RubyTemplate(_input);
			Template.AddRequire("System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

			StringBuilder script = new StringBuilder();
			var methodNames = new Stack<string>();
			BuildScript(script, methodNames);
			var methodsCounter = methodNames.Count;
			while (methodNames.Count > 0)
			{
				script.Append(methodNames.Pop());
				if (methodNames.Count > 0)
					script.Append(" { ");
			}
			if (methodsCounter > 1)
				script.Append(new String('}', methodsCounter - 1));

			try
			{
				Debug.WriteLine(script.ToString());
				ScriptSource source = rubyengine.CreateScriptSourceFromString(script.ToString());
				source.Execute(scope);
			}
			catch (Exception e)
			{
				throw new MonoRailException(script.ToString(), e);
			}
		}

		protected void BuildScript(StringBuilder script, Stack<string> methodNames)
		{
			var methodName = FileNameToMethodName("render_", FileName);
			// Skipping genertating the same method
			if (!methodNames.Contains(methodName))
				Template.ToScriptMethod(methodName, script);
			methodNames.Push(methodName);

			if (Parent != null)
				Parent.BuildScript(script, methodNames);
		}

		private static string FileNameToMethodName(string prefix, string fileName)
		{
			// TODO: Optimize this
			return string.Concat(prefix, fileName.ToLower().Replace("\\", "_").Replace(".", "_"));
		}
	}
}