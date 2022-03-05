using buyitWeb.Models;
using buyitWeb.Repository;
using Microsoft.AspNetCore.Mvc;

namespace buyitWeb.Controllers
{
    public class CoverTypeController : Controller
    {
        [BindProperty]
        public CoverTypeModel coverTypeModel { get; set; }
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverTypeModel> coverTypes = _unitOfWork.CoverType.GetAll();
            return View(coverTypes);
        }

        [HttpPost]
        public IActionResult Create()
        {
            _unitOfWork.CoverType.Add(coverTypeModel);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            _unitOfWork.CoverType.Remove(coverTypeModel);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
