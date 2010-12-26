using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ToBeSeen.Plumbing;

namespace ToBeSeen.Installers
{
	public class PersistenceInstaller : IWindsorInstaller
    {
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.AddFacility<StartableFacility>(f => f.DeferredStart());
			container.AddFacility<PersistenceFacility>();
		}
    }
}