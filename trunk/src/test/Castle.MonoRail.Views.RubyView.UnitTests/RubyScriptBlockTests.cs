using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;

namespace Castle.MonoRail.Views.RubyView.UnitTests
{
	[TestFixture]
	public class RubyScriptBlockTests : NUnitTestFixture
	{
		[Test]
		public void Parse_MultiLine_Test()
		{
			const string text = @"Some" + "\n" +
								@"multi line" + "\n" +
								@"text.";

			var block = RubyScriptBlock.Parse(text);
			Assert.That(block.Content, Text.StartsWith(RubyScriptBlock.OUTPUT));
			Assert.That(block.Content, Text.Contains("\ntext.\""));
		}

		[Test]
		public void Parse_TextWithQuotes_Test()
		{
			const string text = @"Some ""quotes"" and this 'one'." + "\n" +
								@"multi line" + "\n" +
								@"text.";

			var block = RubyScriptBlock.Parse(text);
			Assert.That(block.Content, Text.StartsWith(RubyScriptBlock.OUTPUT));
			Assert.That(block.Content, Text.Contains("\\\"quotes\\\""));
			Assert.That(block.Content, Text.Contains("'one'"));
			Assert.That(block.Content, Text.Contains("\ntext.\""));
		}

		[Test]
		public void Parse_OutputObjectWithProperty_Test()
		{
			const string text = @"<%= Time.now %>";

			var block = RubyScriptBlock.Parse(text);
			Assert.That(block.Content, Text.StartsWith(RubyScriptBlock.OUTPUT));
			Assert.That(block.Content, Text.Contains("Time.now"));
		}

		[Test]
		public void Parse_BlockWithCode_Test()
		{
			const string text = @"<% for something %>";

			var block = RubyScriptBlock.Parse(text);
			Assert.That(block.Content, Text.Matches("for something"));
		}

	}
}