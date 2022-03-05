﻿using buyitWeb.Data;
using buyitWeb.Models;
using buyitWeb.Repository;
using buyitWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace buyitWeb.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Models.CategoryModel> category = _unitOfWork.Category.GetAll(); // no need for ToList(), wow
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Models.CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Models.CategoryModel category)
        {
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}