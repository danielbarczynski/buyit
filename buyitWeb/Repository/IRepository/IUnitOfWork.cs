using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public interface IUnitOfWork 
    {
        ICategory Category { get; }
        ICoverType CoverType { get; }
        void Save();
    }
}
