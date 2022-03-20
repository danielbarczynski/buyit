using buyitWeb.Data;
using buyitWeb.Models;
using buyitWeb.Repository;
using Microsoft.AspNetCore.Mvc;

namespace buyitWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _applicationDbContext;
        public OrderController(ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork)
        {
            _applicationDbContext = applicationDbContext;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<OrderHeaderModel> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(properties: "ApplicationUser");
            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
