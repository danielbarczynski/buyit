using System.Linq.Expressions;

namespace buyitWeb.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string properties = null);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string properties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
