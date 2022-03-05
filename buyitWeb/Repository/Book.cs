using buyitWeb.Data;
using buyitWeb.Models;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public class Book : Repository<BookModel>, IBook
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public Book(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
