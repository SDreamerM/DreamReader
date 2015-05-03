using System.Web.Mvc;
using DreamReader.Web.Models;

namespace DreamReader.Web.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetDreamReaderViewModel()
        {
            var result = new DreamReaderViewModel();
            result.IsAuthenticated = User.Identity.IsAuthenticated;
            return JsonSuccess(result);
        }
    }
}