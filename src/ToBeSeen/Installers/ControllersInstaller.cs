using System;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ToBeSeen.Controllers;

namespace ToBeSeen.Installers
{
	public class ControllersInstaller:IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(AllTypes.FromThisAssembly()
		                   		.BasedOn<IController>()
		                   		.If(Component.IsInSameNamespaceAs<HomeController>())
		                   		.If(t => t.Name.EndsWith("Controller"))
		                   		.Configure((ConfigureDelegate) (c => c.Named(c.ServiceType.Name)
		                   	                                     		.LifeStyle.Transient)));
		}
	}
}