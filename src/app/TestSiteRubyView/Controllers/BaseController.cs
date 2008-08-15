using Castle.MonoRail.Framework;

namespace TestSiteRubyView.Controllers
{
	[DefaultAction]
	public abstract class BaseController : Controller
	{
		const string STR_NOTFOUND_TEMPLATE = "/Rescues/TemplateNotFound";

		/// <summary>
		/// Default action for all controllers. This action is run when passing
		/// action for controller doesn't exists. It can be overridden.
		/// <remarks>DefaultAction checks if passed action exists as view. If not 
		/// STR_NOTFOUND_TEMPLATE is rendered.</remarks>
		/// </summary>
		public virtual void DefaultAction()
		{
			var template = STR_NOTFOUND_TEMPLATE;
			var path = string.Concat(ViewFolder,"/",Action);
			if(string.IsNullOrEmpty(AreaName))
				path = string.Concat(ViewFolder, ".", AreaName, "/", Action);

			if (HasTemplate(path))
			{
				template = Action;
			}
			else
			{
				Context.Response.StatusCode = 404;
				LayoutName = "Rescue";
			}
			RenderView(template);
		}
	}
}