using buyitWeb.Data;
using buyitWeb.Models;

namespace buyitWeb.Repository
{
    public class Cart : Repository<CartModel>, ICart
    {
        private ApplicationDbContext _applicationDbContext;

        public Cart(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            ;
        }

        public int DecrementCount(CartModel shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }

        public int IncrementCount(CartModel shoppingCart, int count)
        {
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }
    }
}

