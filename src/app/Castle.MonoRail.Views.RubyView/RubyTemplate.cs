using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Castle.MonoRail.Views.RubyView
{
	public class RubyTemplate
	{
		string template;
		List<string> requires = new List<string>();

		public RubyTemplate(TextReader input)
		{
			if (input == null)
				throw new ArgumentNullException("input", "Cannot pass null templateContents to the constructor");

			template = input.ReadToEnd();
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
			string contents = template;
			builder.AppendLine();
			requires.ForEach(require => builder.AppendLine(string.Format("require '{0}'", require)));

			if (!String.IsNullOrEmpty(methodName))
			{
				builder.AppendLine("def " + methodName);
			}
			Regex scriptBlocks = new Regex("<%.*?%>", RegexOptions.Compiled | RegexOptions.Singleline);
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
	}
}