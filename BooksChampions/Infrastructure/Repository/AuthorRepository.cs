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
    }
}
