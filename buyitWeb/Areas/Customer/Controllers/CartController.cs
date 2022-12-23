using buyitWeb.Models;
using buyitWeb.Models.ViewModels;
using buyitWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace buyitWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public CartVM CartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
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

            var domain = "https://localhost:44361/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={CartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in CartVM.Cart)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.BookModel.Price * 100),
                        Currency = "pln",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.BookModel.Title
                        },

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentID(CartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            //_unitOfWork.Cart.RemoveRange(CartVM.Cart);
            //_unitOfWork.Save();

            //return RedirectToAction("Index","Home");
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeaderModel orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id, properties: "ApplicationUser");
            if (orderHeader.PaymentStatus != Statuses.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStatus(id, Statuses.StatusApproved, Statuses.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order", "<p>New Order Created</p>");
            List<CartModel> shoppingCarts = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId ==
            orderHeader.ApplicationUserId).ToList();
            _unitOfWork.Cart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }
    }
}
