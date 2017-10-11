using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.TaskCore.Repository
{
    public interface IRepository<T, TKey>
    {
        Task<T> Create(T task);
        Task Delete(TKey id);
        Task<IEnumerable<T>> Get();
        Task<IEnumerable<T>> Get(string userId);
        Task<T> Get(TKey id);
        Task<T> Update(T task);
    }
}