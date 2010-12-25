using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace ToBeSeen.Plumbing
{
	public class WindsorControllerFactory: DefaultControllerFactory
	{
		private readonly IKernel kernel;

		public WindsorControllerFactory(IKernel kernel)
		{
			this.kernel = kernel;
		}

		public override void ReleaseController(IController controller)
		{
			kernel.ReleaseComponent(controller);
		}

		public override IController CreateController(RequestContext requestContext, string controllerName)
		{
			var controllerComponentName = controllerName + "controller";
			return kernel.Resolve<IController>(controllerComponentName);
		}
	}
}