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

        public int DecrementCount(CartModel cartModel, int count)
        {
            cartModel.Count -= count;
            return cartModel.Count;
        }

        public int IncrementCount(CartModel cartModel, int count)
        {
            cartModel.Count += count;
            return cartModel.Count;
        }
    }
}

