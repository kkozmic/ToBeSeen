using Castle.Core;
using NHibernate;

namespace ToBeSeen.Plumbing
{
    /// <summary>
    /// Provides access to session factory
    /// </summary>
    public interface ISessionFactoryProvider : IInitializable
    {
        /// <summary>
        /// Returns an instance of SessionFactory
        /// </summary>
        ISessionFactory SessionFactory { get; }
    }
}