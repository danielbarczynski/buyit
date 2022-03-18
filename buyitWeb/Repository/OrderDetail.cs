using buyitWeb.Data;
using buyitWeb.Models;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public class OrderDetail : Repository<OrderDetailModel>, IOrderDetail
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public OrderDetail(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public void Update(OrderDetailModel orderDetailModel)
        {
            _applicationDbContext.OrderDetails.Update(orderDetailModel);
        }
    }
}
