using System.Web.Mvc;
using NHibernate;

namespace ToBeSeen.Controllers
{
	public class HomeController : Controller
	{
		private readonly ISession session;

		public HomeController(ISession session)
		{
			this.session = session;
		}

		public ActionResult Index()
		{
			ViewModel.Message = "Welcome to ToBeSeen WebSite!";
			ViewModel.EventCount = session.QueryOver<Event>().RowCount();

			return View();
		}

		public ActionResult About()
		{
			return View();
		}
	}
}