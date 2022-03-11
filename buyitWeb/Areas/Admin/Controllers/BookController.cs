using buyitWeb.Models;
using buyitWeb.Models.ViewModels;
using buyitWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace buyitWeb.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        [BindProperty]
        public BookModel bookModel { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public readonly IWebHostEnvironment _hostEnvironment;

        public BookController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            //IEnumerable<BookModel> coverTypes = _unitOfWork.Book.GetAll();
            //return View(coverTypes);
            return View();
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
        public IActionResult Create(BookVM bookVM, IFormFile file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\books");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                bookVM.BookModel.ImageUrl = @"\images\books\" + fileName + extension;

            }
            _unitOfWork.Book.Add(bookVM.BookModel);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _unitOfWork.Book.GetAll(properties:"Category,CoverType");
            return Json(new { data = books });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.Book.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            //if (System.IO.File.Exists(oldImagePath))
            //{
            //    System.IO.File.Delete(oldImagePath);
            //}

            _unitOfWork.Book.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
