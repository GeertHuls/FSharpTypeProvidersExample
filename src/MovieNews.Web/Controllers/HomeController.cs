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

        public async Task<ActionResult> Details(string title)
        {
	        var details = await Movies.GetMovieInfo(title);
	        if (details.HasMovie)
	        {
		        return View(details.Movie);
	        }

            return View("MovieNotFound");
        }
    }
}