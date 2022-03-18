using buyitWeb.Models;

namespace buyitWeb.Repository.IRepository
{
    public interface IOrderHeader : IRepository<OrderHeaderModel>
    {
        void Update(OrderHeaderModel orderHeader);
        void UpdateStatus(int id, string orderStatus, string paymentStatus=null);
    }
}
