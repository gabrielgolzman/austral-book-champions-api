using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _dbContext;

        public BookRepository(BookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Book>> GetBooks()
        {
            return await _dbContext.Books
                .Include(x => x.Authors)
                .ToListAsync();
        }

        public async Task<int> AddBook(Book book)
        {
            if (book.Authors != null && book.Authors.Any())
            {
                foreach (var author in book.Authors)
                {
                    _dbContext.Entry(author).State = EntityState.Unchanged;
                }
            }

            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            return book.Id;
        }

        public async Task DeleteBook(int id) {
            var book =  await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book != null) {
                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
