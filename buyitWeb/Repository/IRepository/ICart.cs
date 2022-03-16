using buyitWeb.Models;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public interface ICart : IRepository<CartModel>
    {
        public interface IShoppingCartRepository : IRepository<CartModel>
        {
            int IncrementCount(CartModel shoppingCart, int count);
            int DecrementCount(CartModel shoppingCart, int count);
        }
    }
}
