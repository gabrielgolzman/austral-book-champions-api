﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly BookDbContext _dbContext;

        public BaseRepository( BookDbContext bookDbContext)
        {
            _dbContext = bookDbContext;
        }

        public List<T> Get()
        {
            return _dbContext.Set<T>().ToList();
        }

        public T Get<TId>(TId id)
        {
            return _dbContext.Set<T>().Find(new object[] {id});
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
