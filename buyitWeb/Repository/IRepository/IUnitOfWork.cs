using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public interface IUnitOfWork 
    {
        ICategory Category { get; }
        void Save();
    }
}
