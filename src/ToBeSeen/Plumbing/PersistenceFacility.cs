using System;
using Castle.Core.Internal;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace ToBeSeen.Plumbing
{
	public class PersistenceFacility : AbstractFacility
	{
		protected override void Init()
		{
			var config = BuildDatabaseConfiguration();

			Kernel.Register(
				Component.For<ISessionFactory>()
					.UsingFactoryMethod(config.BuildSessionFactory),
				Component.For<ISession>()
					.UsingFactoryMethod(k => k.Resolve<ISessionFactory>().OpenSession())
					.LifeStyle.PerWebRequest);
		}

		private Configuration BuildDatabaseConfiguration()
		{
			return Fluently.Configure()
				.Database(SetupDatabase)
				.Mappings(m => m.AutoMappings.Add(CreateMappingModel()))
				.ExposeConfiguration(ConfigurePersistence)
				.BuildConfiguration();
		}

		protected virtual AutoPersistenceModel CreateMappingModel()
		{
			var m = AutoMap.Assembly(typeof (EntityBase).Assembly)
				.Where(IsDomainEntity)
				.OverrideAll(ShouldIgnoreProperty)
				.IgnoreBase<EntityBase>();

			return m;
		}

		protected virtual IPersistenceConfigurer SetupDatabase()
		{
			return MsSqlConfiguration.MsSql2008
				.UseOuterJoin()
				.ProxyFactoryFactory(typeof (ProxyFactoryFactory))
				.ConnectionString(x => x.FromConnectionStringWithKey("ApplicationServices"))
				.ShowSql();
		}

		protected virtual void ConfigurePersistence(Configuration config)
		{
			SchemaMetadataUpdater.QuoteTableAndColumns(config);
		}

		protected virtual bool IsDomainEntity(Type t)
		{
			return typeof (EntityBase).IsAssignableFrom(t);
		}

		private void ShouldIgnoreProperty(IPropertyIgnorer property)
		{
			property.IgnoreProperties(p => p.MemberInfo.HasAttribute<DoNotMapAttribute>());
		}
	}
}