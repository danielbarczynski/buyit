using buyitWeb.Models;
using buyitWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace buyitWeb.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        [BindProperty]
        public CoverTypeModel coverTypeModel { get; set; }
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Index()
        {
            IEnumerable<CoverTypeModel> coverTypes = _unitOfWork.CoverType.GetAll();
            return View(coverTypes);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public IActionResult Create()
        {
            _unitOfWork.CoverType.Add(coverTypeModel);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public IActionResult Delete()
        {
            _unitOfWork.CoverType.Remove(coverTypeModel);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
