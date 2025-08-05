using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {

        public AuthorRepository(BookDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Author>> GetAuthorsByIds(List<int> authorIds)
        {
            return await _dbContext.Authors
                .Where(a => authorIds.Contains(a.Id))
                .ToListAsync();
        }
    }
}
