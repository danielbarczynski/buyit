using buyitWeb.Models;
using buyitWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace buyitWeb.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<BookModel> bookModel = _unitOfWork.Book.GetAll(properties: "Category,CoverType");
            return View(bookModel);
        }

        //public IActionResult Index(int id)
        //{
        //    CartModel bookModel = new CartModel()
        //    {
        //        Count = 1,
        //        BookModel = _unitOfWork.Book.GetFirstOrDefault(u => u.Id == id, properties: "Category,CoverType"),
        //    };
        //    return View(bookModel);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int bookId)
        {
            CartModel cartObj = new()
            {
                Count = 1,
                BookModelId = bookId,
                BookModel = _unitOfWork.Book.GetFirstOrDefault(u => u.Id == bookId, properties: "Category,CoverType"),
            };

            return View(cartObj);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Add(CartModel cartModel)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cartModel.ApplicationUserId = claim.Value;

            CartModel cartFromDb = _unitOfWork.Cart.GetFirstOrDefault(
     u => u.ApplicationUserId == claim.Value && u.BookModelId == cartModel.BookModelId);

            if (cartFromDb == null)
            {
                _unitOfWork.Cart.Add(cartModel);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.Cart.IncrementCount(cartFromDb, cartModel.Count);
                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }
    }
}
