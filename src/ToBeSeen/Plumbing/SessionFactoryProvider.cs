using System;
using NHibernate;
using NHibernate.Cfg;

namespace ToBeSeen.Plumbing
{
	public class SessionFactoryProvider : ISessionFactoryProvider, IDisposable
	{
		private readonly Configuration config;

		public SessionFactoryProvider(Configuration config)
		{
			this.config = config;
		}

		public virtual ISessionFactory SessionFactory
		{
			get;
			private set;
		}

		public virtual void Initialize()
		{
			SessionFactory = config.BuildSessionFactory();
		}

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual bool IsDisposed
		{
			get;
			private set;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (IsDisposed) 
				return;

			if (disposing)
			{
				if (SessionFactory != null)
				{
					SessionFactory.Close();
					SessionFactory = null;
				}
			}

			IsDisposed = true;
		}
	}
}