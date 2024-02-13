using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly LeaveManagementDbContext _dbContext;
        public GenericRepository(LeaveManagementDbContext context)
        {

            _dbContext = context;

        }
        public async Task<T> AddAsync(T enity)
        {
            await _dbContext.AddAsync(enity);
            return enity;
        }

        public async Task DeleteAsync(T entity)
        {
           _dbContext.Set<T>().Remove(entity);
          

        }
        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T enity)
        {
            _dbContext.Entry(enity).State = EntityState.Modified;
          
        }
    }
}
