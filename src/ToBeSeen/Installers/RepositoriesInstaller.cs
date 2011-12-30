using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using ToBeSeen.Repositories;

namespace ToBeSeen.Installers
{
	public class RepositoriesInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Classes.FromThisAssembly()
			                   	.Where(Component.IsInSameNamespaceAs<EventRepository>())
			                   	.WithService.DefaultInterfaces()
			                   	.LifestyleTransient()
			                   	.Configure(c => c.DependsOn(new { pageSize = 20 })));
		}
	}
}