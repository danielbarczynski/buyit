using buyitWeb.Models;
using buyitWeb.Repository;
using Microsoft.AspNetCore.Mvc;

namespace buyitWeb.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        [BindProperty]
        public BookModel bookModel { get; set; }
        private readonly IUnitOfWork _unitOfWork;

        public BookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<BookModel> coverTypes = _unitOfWork.Book.GetAll();
            return View(coverTypes);
        }

        [HttpPost]
        public IActionResult Create()
        {
            _unitOfWork.Book.Add(bookModel);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            _unitOfWork.Book.Remove(bookModel);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
