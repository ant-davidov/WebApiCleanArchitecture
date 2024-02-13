using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<bool> Exists (int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T enity);
        Task  UpdateAsync(T enity);
        Task  DeleteAsync(T  entity);
    }
}
