using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _dbContext;

        public BookRepository(BookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Book> GetBooks()
        {
            return _dbContext.Books.ToList();
        }

        public int AddBook(Book book)
        {
            _dbContext.Books.Add(book);

            _dbContext.SaveChanges();

            return book.Id;
        }

        public void DeleteBook(int id) {
            var book = _dbContext.Books.FirstOrDefault(x => x.Id == id);

            if (book != null) {
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
            }
        }
    }
}
