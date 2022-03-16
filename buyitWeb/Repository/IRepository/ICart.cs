using buyitWeb.Models;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public interface ICart : IRepository<CartModel>
    { 
            int IncrementCount(CartModel cartModel, int count);
            int DecrementCount(CartModel cartModel, int count);       
    }
}
