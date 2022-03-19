using buyitWeb.Models;
using buyitWeb.Models.ViewModels;
using buyitWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace buyitWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public CartVM CartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM = new CartVM()
            {
                Cart = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == claim.Value,
                properties: "BookModel"),
                OrderHeader = new(),
            };

            foreach (var cart in CartVM.Cart)
            {
                CartVM.OrderHeader.OrderTotal += (cart.Count * cart.BookModel.Price);
                cart.BookModel.ItemTotal = (cart.Count * cart.BookModel.Price);
                cart.BookModel.ItemTotal = Math.Round(cart.BookModel.ItemTotal, 2);
                CartVM.OrderHeader.OrderTotal = Math.Round(CartVM.OrderHeader.OrderTotal, 2);
            }
            return View(CartVM);

        }
        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.Cart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.Id == cartId);
            if (cart.Count >= 2)
            {
                _unitOfWork.Cart.DecrementCount(cart, 1);
            }
            else
            {
                _unitOfWork.Cart.Remove(cart);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.Cart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM = new CartVM()
            {
                Cart = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == claim.Value,
                properties: "BookModel"),
                OrderHeader = new()
            };
            CartVM.OrderHeader.ApplicationUser = _unitOfWork.User.GetFirstOrDefault(
                u => u.Id == claim.Value);

            CartVM.OrderHeader.Name = CartVM.OrderHeader.ApplicationUser.Name;
            CartVM.OrderHeader.PhoneNumber = CartVM.OrderHeader.ApplicationUser.PhoneNumber;
            CartVM.OrderHeader.StreetAddress = CartVM.OrderHeader.ApplicationUser.StreetAddress;
            CartVM.OrderHeader.City = CartVM.OrderHeader.ApplicationUser.City;
            CartVM.OrderHeader.State = CartVM.OrderHeader.ApplicationUser.State;
            CartVM.OrderHeader.PostalCode = CartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in CartVM.Cart)
            {
                CartVM.OrderHeader.OrderTotal += (cart.Count * cart.BookModel.Price);
                cart.BookModel.ItemTotal = (cart.Count * cart.BookModel.Price);
                cart.BookModel.ItemTotal = Math.Round(cart.BookModel.ItemTotal, 2);
                CartVM.OrderHeader.OrderTotal = Math.Round(CartVM.OrderHeader.OrderTotal, 2);
            }
            return View(CartVM);
        }

        [HttpPost]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM.Cart = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == claim.Value,
            properties: "BookModel");

            CartVM.OrderHeader.OrderDate = DateTime.Now;
            CartVM.OrderHeader.ApplicationUserId = claim.Value;

            foreach (var cart in CartVM.Cart)
            {
                CartVM.OrderHeader.OrderTotal += (cart.Count * cart.BookModel.Price);
                cart.BookModel.ItemTotal = (cart.Count * cart.BookModel.Price);
                cart.BookModel.ItemTotal = Math.Round(cart.BookModel.ItemTotal, 2);
                CartVM.OrderHeader.OrderTotal = Math.Round(CartVM.OrderHeader.OrderTotal, 2);
            }

            ApplicationUser applicationUser = _unitOfWork.User.GetFirstOrDefault(u => u.Id == claim.Value);

            CartVM.OrderHeader.PaymentStatus = Statuses.PaymentStatusPending;
            CartVM.OrderHeader.OrderStatus = Statuses.StatusPending;

            _unitOfWork.OrderHeader.Add(CartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in CartVM.Cart)
            {
                OrderDetailModel orderDetail = new()
                {
                    BookModelId = cart.BookModelId,
                    OrderId = CartVM.OrderHeader.Id,
                    Price = cart.BookModel.Price,
                    Count = cart.Count,
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            _unitOfWork.Cart.RemoveRange(CartVM.Cart);
            _unitOfWork.Save();

            return RedirectToAction("Index","Home");
        }
    }
}
