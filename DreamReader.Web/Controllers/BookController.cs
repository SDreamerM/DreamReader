using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using DreamReader.Business.Definitions;
using DreamReader.Web.Models;
using Microsoft.AspNet.Identity;

namespace DreamReader.Web.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookManager _bookManager;

        public BookController(IBookManager bookManager)
        {
            _bookManager = bookManager;
        }

        public JsonResult GetBooks()
        {
            var books = _bookManager.GetBooks(User.Identity.GetUserId());
            return JsonSuccess(books.Select(Mapper.DynamicMap<BookViewModel>));
        }

        public JsonResult GetFullBook(long bookId)
        {
            var book = _bookManager.GetFullBook(bookId);
            return JsonSuccess(Mapper.DynamicMap<BookViewModel>(book));
        }

        [HttpPost]
        public JsonResult UploadBook()
        {
            var file = Request.Files[0];
            _bookManager.UploadBook(file.InputStream, User.Identity.GetUserId());
            return JsonSuccess(true);
        }
    }
}