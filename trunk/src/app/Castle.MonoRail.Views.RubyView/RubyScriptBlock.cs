using System;

namespace Castle.MonoRail.Views.RubyView
{
	/// <summary>
	/// Simple parser for single code block.
	/// It supports such code blocks:
	/// <code><%= %></code>
	/// <code><% %></code>
	/// <code><% -%></code>
	/// </summary>
	public class RubyScriptBlock
	{
		/// <summary>
		/// Function definition for output writing.
		/// </summary>
		public const string OUTPUT = "output_buffer.write(";

		private const string OUTPUT_FORMAT = OUTPUT + "{0})";
		[ThreadStatic] private static bool _ignoreNextNewLine; /* false */

		#region constructors ...

		/// <summary>
		/// Initializes a new instance of the <see cref="RubyScriptBlock"/> class.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <exception cref="InvalidOperationException">Exception for incomplete code blocks.</exception>
		private RubyScriptBlock(string block)
		{
			// TODO: Optimize this and write performance tests
			// TODO: Use streams instead of string
			bool ignoreNewLine = _ignoreNextNewLine;

			if (String.IsNullOrEmpty(block))
			{
				Content = string.Empty;
				return;
			}

			int endOffset = 4;
			if (block.EndsWith("-%>"))
			{
				endOffset = 5;
				_ignoreNextNewLine = true;
			}
			else
				_ignoreNextNewLine = false;

			if (block.StartsWith("<%="))
			{
				int outputLength = block.Length - endOffset - 1;
				if (outputLength < 1)
					throw new InvalidOperationException("Started a '<%=' block without ending it.");

				string output = block.Substring(3, outputLength).Trim();
				Content = String.Format(OUTPUT_FORMAT, output).Trim();
				return;
			}

			if (block.StartsWith("<%"))
			{
				Content = block.Substring(2, block.Length - endOffset).Trim();
				return;
			}

			if (ignoreNewLine && block.StartsWith(Environment.NewLine))
				block = block.Remove(0, Environment.NewLine.Length);

			block = block.Replace(@"\", @"\\");
			block = block.Replace(Environment.NewLine, "\\r\\n");
			block = block.Replace(@"""", @"\""");
			if (block.Length > 0)
				Content = String.Format(OUTPUT_FORMAT, string.Concat("\"", block, "\""));
		}

		#endregion

		/// <summary>
		/// Gets parsed content.
		/// </summary>
		/// <value>The content.</value>
		public string Content { get; private set; }

		/// <summary>
		/// Factory method which creates <see cref="RubyScriptBlock"/> object
		/// with parsed content in <see cref="Content"/>.
		/// </summary>
		/// <param name="block">The block to parse.</param>
		/// <returns><see cref="RubyScriptBlock"/> object</returns>
		public static RubyScriptBlock Parse(string block)
		{
			return new RubyScriptBlock(block);
		}
	}
}