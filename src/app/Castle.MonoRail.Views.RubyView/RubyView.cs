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
	public class RubyView : IDisposable
	{
		private  string _fileName;
		private  TextWriter _output;
		private  ScriptRuntime _scriptRuntime;
		//private TextReader _input;
		private RubyTemplateParser _templateParser;

		#region constructors ...

		public RubyView(string fileName, TextWriter output, IViewSource viewSource, ScriptRuntime scriptRuntime)
		{
			ViewSource = viewSource;
			_fileName = fileName;
			_output = output;
			_scriptRuntime = scriptRuntime;
		}

		~RubyView()
		{
			Debug.WriteLine(String.Format("{0} : ~RubyView- {1}", GetType(), "START"));
		}

		public void Dispose()
		{
			if (Parent != null)
			{
				Parent.Dispose();
				Parent = null;
			}
			if (TemplateParser != null)
			{
				TemplateParser.Dispose();
				TemplateParser = null;
			}
			ViewSource = null;
			_output = null;
			_scriptRuntime = null;

		}

		#endregion

		public IViewSource ViewSource { get; set; }

		public string FileName
		{
			get { return _fileName; }
		}

		public RubyView Parent { get; set; }

		public RubyTemplateParser TemplateParser
		{
			get
			{
				if (_templateParser == null)
					_templateParser = new RubyTemplateParser(ViewSource);
				return _templateParser;
			}
			set { _templateParser = value; }
		}

		public void Render()
		{
			var scriptEngine = IronRuby.GetEngine(_scriptRuntime);
			ScriptScope scope = scriptEngine.CreateScope();
			scope.SetVariable("output_buffer", _output);

			TemplateParser = new RubyTemplateParser(ViewSource);
			TemplateParser.AddRequire("System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

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
			script.AppendLine();

			try
			{
				Debug.WriteLine(script.ToString());
				ScriptSource source = scriptEngine.CreateScriptSourceFromString(script.ToString());
				source.Execute(scope);
			}
			catch (Exception e)
			{
				throw new MonoRailException(script.ToString(), e);
			}
			finally
			{
				// TODO : check if remove variable isnt faster.
				scope.ClearVariables();
				scriptEngine.Shutdown();
			}
		}

		protected void BuildScript(StringBuilder script, Stack<string> methodNames)
		{
			var methodName = FileNameToMethodName("render_", FileName);
			// Skipping genertating the same method
			if (!methodNames.Contains(methodName))
				TemplateParser.ToScriptMethod(methodName, script);
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