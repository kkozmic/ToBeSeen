namespace ToBeSeen.Repositories
{
	using ToBeSeen.Plumbing;

	public interface IEventRepository
	{
		Page<Event> GetPage(int pageNumber);
	}
}