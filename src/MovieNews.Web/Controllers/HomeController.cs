using System.Web.Mvc;
using MovieNews.Data;

namespace MovieNews.Web.Controllers
{
	public class HomeController : Controller
    {
        public ActionResult Index()
        {
	        var movies = Movies.GetLatestMovies().Result;
            return View(movies);
        }

        public ActionResult Details(string title)
        {
            return View();
        }
    }
}