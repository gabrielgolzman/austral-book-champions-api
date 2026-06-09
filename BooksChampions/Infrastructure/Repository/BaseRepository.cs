using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly BookDbContext _dbContext;

        public BaseRepository( BookDbContext bookDbContext)
        {
            _dbContext = bookDbContext;
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> Get<TId>(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(new object[] {id});
        }

        public void Add(T item)
        {
            _dbContext.Set<T>().Add(item);
        }

        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }
    }
}
