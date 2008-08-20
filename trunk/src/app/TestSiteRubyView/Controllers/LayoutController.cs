using Castle.MonoRail.Framework;

namespace TestSiteRubyView.Controllers
{
	[Layout("Primary")]
	public class LayoutController : BaseController
	{
		public void NonExistingView()
		{
		}

		public void ExistingView()
		{
			RenderView("Simple", "ExistingView");
		}

		public void OutputString()
		{
			RenderView("Simple", "OutputString");
		}

		public void OutputExpression()
		{
			RenderView("Simple", "OutputExpression");
		}

		public void PutsString()
		{
			RenderView("Simple", "PutsString");
		}

		[Layout("Primary", "Secondary")]
		public void DifferentMultiplyLayouts()
		{
			RenderView("Simple", "OutputString");
		}

		[Layout("Primary", "Primary")]
		public void SameMultiplyLayouts()
		{
			RenderView("Simple", "OutputString");
		}

	}
}