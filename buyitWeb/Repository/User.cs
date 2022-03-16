using buyitWeb.Data;
using buyitWeb.Models;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public class User : Repository<ApplicationUser>, IUser
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public User(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
