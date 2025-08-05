using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookDbContext _dbContext;

        public AuthorRepository(BookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Author>> GetAuthors()
        {
            return await _dbContext.Authors.ToListAsync();
        }

        public async Task<List<Author>> GetAuthorsByIds(List<int> authorIds)
        {
            return await _dbContext.Authors
                .Where(a => authorIds.Contains(a.Id))
                .ToListAsync();
        }
    }
}
