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

			//Provided controllerName could be things such as "favicon.ico" 
			//so checking availability of the component name before resolve
			if(kernel.HasComponent(controllerComponentName))
			{
				return kernel.Resolve<IController>(controllerComponentName);
			}

			throw new ComponentNotFoundException(controllerComponentName);
		}
	}
}