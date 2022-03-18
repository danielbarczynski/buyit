using buyitWeb.Data;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ICategory Category { get; private set; }
        public ICoverType CoverType { get; private set; }
        public IBook Book { get; private set; }
        public ICart Cart { get; private set; }
        public IOrderHeader OrderHeader { get; private set; }
        public IOrderDetail OrderDetail { get; private set; }
        public IUser User { get; private set; }

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            Category = new Category(_applicationDbContext);
            CoverType = new CoverType(_applicationDbContext);
            Book = new Book(_applicationDbContext);
            Cart = new Cart(_applicationDbContext);
            OrderHeader = new OrderHeader(_applicationDbContext);
            OrderDetail = new OrderDetail(_applicationDbContext);
            User = new User(_applicationDbContext);
        }


        public void Save()
        {
            _applicationDbContext.SaveChanges();
        }
    }
}
