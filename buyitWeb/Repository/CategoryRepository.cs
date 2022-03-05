using buyitWeb.Data;
using buyitWeb.Models;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public class CategoryRepository : Repository<Category>, ICategory
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CategoryRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
