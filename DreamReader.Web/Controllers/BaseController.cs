using System.Web.Mvc;

namespace DreamReader.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = JsonFailure(filterContext.Exception.Message);
            filterContext.ExceptionHandled = true;
        }

        protected JsonResult JsonSuccess(object data)
        {
            var result = new { result = true, data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult JsonFailure(string message)
        {
            var result = new { result = false, message };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}