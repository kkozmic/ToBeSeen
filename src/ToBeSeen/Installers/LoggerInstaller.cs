using System;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace ToBeSeen.Installers
{
	public class LoggerInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.AddFacility("logging", new LoggingFacility(LoggerImplementation.Log4net));
		}
	}
}