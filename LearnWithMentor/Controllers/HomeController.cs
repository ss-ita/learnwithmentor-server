using System.Web.Mvc;
using LearnWithMentor.Filters;

namespace LearnWithMentor.Controllers
{
    [System.Web.Http.Authorize]
    [JwtAuthentication]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
