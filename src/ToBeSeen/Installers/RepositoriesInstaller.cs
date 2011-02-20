namespace ToBeSeen.Installers
{
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.SubSystems.Configuration;
	using Castle.Windsor;

	using ToBeSeen.Repositories;

	public class RepositoriesInstaller:IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(AllTypes.FromThisAssembly()
								.Where(Component.IsInSameNamespaceAs<EventRepository>())
								.WithService.DefaultInterface()
								.Configure(c => c.LifeStyle.Transient
			                   					.DependsOn(new { pageSize = 20 })));
		}
	}
}