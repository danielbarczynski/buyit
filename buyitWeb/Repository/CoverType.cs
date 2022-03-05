using buyitWeb.Data;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public class CoverType : Repository<Models.CoverTypeModel>, ICoverType
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CoverType(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
