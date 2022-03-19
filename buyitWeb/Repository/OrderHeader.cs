using buyitWeb.Data;
using buyitWeb.Models;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public class OrderHeader : Repository<OrderHeaderModel>, IOrderHeader
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public OrderHeader(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public void Update(OrderHeaderModel orderHeaderModel)
        {
            _applicationDbContext.OrderHeaders.Update(orderHeaderModel);
        }

        public void UpdateStatus(int id, string orderStatus, string paymentStatus = null)
        {
            var orderFromDb = _applicationDbContext.OrderHeaders.FirstOrDefault(u=>u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDb = _applicationDbContext.OrderHeaders.FirstOrDefault(u=>u.Id == id);
            orderFromDb.SessionId = sessionId;
            orderFromDb.PaymentIntentId = paymentIntentId;  
        }
    }
}
