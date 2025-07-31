using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBookRepository
    {
        public Task<List<Book>> GetBooks();

        public Task<int> AddBook(Book book);

        public Task DeleteBook(int id);
    }
}
