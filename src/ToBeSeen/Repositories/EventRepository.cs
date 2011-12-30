using NHibernate;

using ToBeSeen.Plumbing;

namespace ToBeSeen.Repositories
{
	public class EventRepository : IEventRepository
	{
		private readonly int pageSize;

		private readonly ISession session;

		public EventRepository(int pageSize, ISession session)
		{
			this.pageSize = pageSize;
			this.session = session;
		}

		public Page<Event> GetPage(int pageNumber)
		{
			var firstResult = pageSize*(pageNumber - 1);
			using (var tx = session.BeginTransaction())
			{
				var totalCount = session.QueryOver<Event>().ToRowCountQuery().FutureValue<int>();
				var events = session.QueryOver<Event>().Take(pageSize).Skip(firstResult).Future();
				var page = new Page<Event>(events, pageNumber, totalCount.Value, pageSize);
				tx.Commit();
				return page;
			}
		}
	}
}