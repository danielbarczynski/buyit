using buyitWeb.Models;

namespace buyitWeb.Repository.IRepository
{
    public interface IOrderDetail : IRepository<OrderDetailModel>
    {
        void Update(OrderDetailModel orderDetailModel);
    }
}
