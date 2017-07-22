using System.Web.Mvc;
using MovieNews.Data;
using System.Threading.Tasks;

namespace MovieNews.Web.Controllers
{
	public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
	        var movies = await Movies.GetLatestMovies();
            return View(movies);
        }

        public ActionResult Details(string title)
        {
            return View();
        }
    }
}