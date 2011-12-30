using ToBeSeen.Plumbing;

namespace ToBeSeen.Repositories
{
	public interface IEventRepository
	{
		Page<Event> GetPage(int pageNumber);
	}
}