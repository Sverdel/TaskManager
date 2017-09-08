using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;

namespace TaskManager.Core.Api.Repository
{
    public interface IRepository<T, TKey>
    {
        Task Create(T task);
        Task Delete(TKey id);
        Task<IEnumerable<T>> Get();
        Task<T> Get(TKey id);
        Task Update(T task);
    }
}