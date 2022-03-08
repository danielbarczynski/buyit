using buyitWeb.Models;
using buyitWeb.Models.ViewModels;
using buyitWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Create()
        {
            BookVM bookVM = new BookVM()
            {
                BookModel = new BookModel(),
                Categories = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }),
                Covers = _unitOfWork.CoverType.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }),
            };
            //ViewBag.categories = categories;
            //ViewBag.covers = covers;
            return View(bookVM);
        }

        [HttpPost]
        public IActionResult Create(BookModel bookModel)
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
