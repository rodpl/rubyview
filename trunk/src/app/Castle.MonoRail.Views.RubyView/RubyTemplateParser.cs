using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Castle.MonoRail.Framework;
using Ruby.Compiler.Ast;

namespace Castle.MonoRail.Views.RubyView
{
	public class RubyTemplateParser : IDisposable
	{
		private IViewSource _sourceLoader;
		List<string> requires = new List<string>();

		public RubyTemplateParser(IViewSource sourceLoader)
		{
			if (sourceLoader == null) throw new ArgumentNullException("sourceLoader");
			_sourceLoader = sourceLoader;
		}

		~RubyTemplateParser()
		{
			Debug.WriteLine(String.Format("{0} : ~RubyTemplateParser- {1}", GetType(), "START"));
		}

		public void AddRequire(string require)
		{
			requires.Add(require);
		}

		public string ToScriptMethod()
		{
			return ToScriptMethod(null);
		}

		/// <summary>
		/// Parses the template and returns 
		/// </summary>
		/// <param name="methodName"></param>
		/// <returns></returns>
		public string ToScriptMethod(string methodName)
		{
			StringBuilder builder = new StringBuilder();
			ToScriptMethod(methodName, builder);
			return builder.ToString().Trim();
		}

		public void ToScriptMethod(string methodName, StringBuilder builder)
		{
			string contents;

			using (var reader = new StreamReader(_sourceLoader.OpenViewStream()))
				contents = reader.ReadToEnd();
			
			builder.AppendLine();
			requires.ForEach(require => builder.AppendLine(string.Format("require '{0}'", require)));

			if (!String.IsNullOrEmpty(methodName))
				builder.AppendLine("def " + methodName);

			// TODO: Change for  precompiled Regex.CompileToAssembly(...) 
			Regex scriptBlocks = new Regex("<%.*?%>", RegexOptions.Singleline);
			MatchCollection matches = scriptBlocks.Matches(contents);

			int currentIndex = 0;
			int blockBeginIndex = 0;
			foreach (Match match in matches)
			{
				blockBeginIndex = match.Index;
				RubyScriptBlock block = RubyScriptBlock.Parse(contents.Substring(currentIndex, blockBeginIndex - currentIndex));
				if (!String.IsNullOrEmpty(block.Content))
				{
					builder.AppendLine(block.Content);
				}

				block = RubyScriptBlock.Parse(match.Value);
				builder.AppendLine(block.Content);
				currentIndex = match.Index + match.Length;
			}

			if (currentIndex < contents.Length - 1)
			{
				RubyScriptBlock endBlock = RubyScriptBlock.Parse(contents.Substring(currentIndex));
				builder.Append(endBlock.Content);
			}

			if (!String.IsNullOrEmpty(methodName))
			{
				builder.AppendLine();
				builder.AppendLine("end");
			}
		}

		public void Dispose()
		{
			_sourceLoader = null;
		}
	}
}