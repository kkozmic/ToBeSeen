namespace ToBeSeenTests
{
	using System;
	using System.Linq;
	using System.Web.Mvc;

	using Castle.Core;
	using Castle.Core.Internal;
	using Castle.MicroKernel;
	using Castle.Windsor;

	using ToBeSeen.Controllers;
	using ToBeSeen.Installers;

	using Xunit;

	public class ControllersInstallerTests
	{
		private readonly IWindsorContainer containerWithControllers;

		public ControllersInstallerTests()
		{
			containerWithControllers = new WindsorContainer()
				.Install(new ControllersInstaller());
		}

		[Fact]
		public void All_and_only_controllers_have_Controllers_suffix()
		{
			var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
			var registeredControllers = GetImplementationTypesFor(typeof(IController), containerWithControllers);
			Assert.Equal(allControllers, registeredControllers);
		}

		[Fact]
		public void All_and_only_controllers_live_in_Controllers_namespace()
		{
			var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Controllers"));
			var registeredControllers = GetImplementationTypesFor(typeof(IController), containerWithControllers);
			Assert.Equal(allControllers, registeredControllers);
		}

		[Fact]
		public void All_controllers_are_registered()
		{
			// Is<TType> is an helper, extension method from Windsor
			// which behaves like 'is' keyword in C# but at a Type, not instance level
			var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Is<IController>());
			var registeredControllers = GetImplementationTypesFor(typeof(IController), containerWithControllers);
			Assert.Equal(allControllers, registeredControllers);
		}

		[Fact]
		public void All_controllers_are_transient()
		{
			var nonTransientControllers = GetHandlersFor(typeof(IController), containerWithControllers)
				.Where(controller => controller.ComponentModel.LifestyleType != LifestyleType.Transient)
				.ToArray();

			Assert.Empty(nonTransientControllers);
		}

		[Fact]
		public void All_controllers_implement_IController()
		{
			var allHandlers = GetAllHandlers(containerWithControllers);
			var controllerHandlers = GetHandlersFor(typeof(IController), containerWithControllers);

			Assert.NotEmpty(allHandlers);
			Assert.Equal(allHandlers, controllerHandlers);
		}

		private IHandler[] GetAllHandlers(IWindsorContainer container)
		{
			return GetHandlersFor(typeof(object), container);
		}

		private IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
		{
			return container.Kernel.GetAssignableHandlers(type);
		}

		private Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
		{
			return GetHandlersFor(type, container)
				.Select(h => h.ComponentModel.Implementation)
				.OrderBy(t => t.Name)
				.ToArray();
		}

		private Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where)
		{
			return typeof(HomeController).Assembly.GetExportedTypes()
				.Where(t => t.IsClass)
				.Where(t => t.IsAbstract == false)
				.Where(where.Invoke)
				.OrderBy(t => t.Name)
				.ToArray();
		}
	}
}