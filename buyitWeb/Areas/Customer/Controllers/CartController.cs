﻿using buyitWeb.Models.ViewModels;
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
            };

            foreach(var cart in CartVM.Cart)
            {
                CartVM.CartTotal += (cart.Count * cart.BookModel.Price);
                cart.BookModel.ItemTotal = (cart.Count * cart.BookModel.Price);
                cart.BookModel.ItemTotal = Math.Round(cart.BookModel.ItemTotal, 2);
            }
            return View(CartVM);

        }
    }
}
