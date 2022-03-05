using buyitWeb.Data;
using buyitWeb.Models;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public class Category : Repository<Models.CategoryModel>, ICategory
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public Category(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
