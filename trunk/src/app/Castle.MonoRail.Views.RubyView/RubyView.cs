using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
		private readonly string _fileName;
		private TextReader _input;

		public RubyView(string fileName, TextReader input, TextWriter output)
		{
			_fileName = fileName;
			_input = input;
			_output = output;
		}

		public string FileName
		{
			get { return _fileName; }
		}

		public RubyView Parent { get; set; }

		private RubyTemplate _template;
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
				if(methodNames.Count > 0)
					script.Append(" { ");
			}
			if (methodsCounter > 1)
				script.Append(new String('}', methodsCounter - 1));

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

		public void BuildScript(StringBuilder script, Stack<string> methodNames)
		{
			var methodName = FileNameToMethodName("render_", FileName);
			// Skipping genertating the same method
			if(!methodNames.Contains(methodName)) 
				Template.ToScriptMethod(methodName, script);
			methodNames.Push(methodName);

			if (Parent != null)
				Parent.BuildScript(script, methodNames);
		}

		private static string FileNameToMethodName(string prefix, string fileName)
		{
			// TODO: Optimize this
			return string.Concat(prefix,fileName.ToLower().Replace("\\","_").Replace(".","_"));
		}
	}
}
