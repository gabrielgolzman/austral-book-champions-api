using Domain.Entities;
using Domain.Interfaces;

namespace Application.Tests.Fakes;

/// <summary>
/// Test fake: a simplified in-memory implementation of IBookRepository.
/// Use when tests care about state/outcomes rather than interaction verification.
/// </summary>
public class FakeBookRepository : IBookRepository
{
    private readonly List<Book> _books = [];
    private int _nextId = 1;

    public IReadOnlyList<Book> Books => _books;

    public void Seed(IEnumerable<Book> books)
    {
        foreach (var book in books)
        {
            _books.Add(book);
            if (book.Id >= _nextId)
            {
                _nextId = book.Id + 1;
            }
        }
    }

    public Task<List<Book>> GetBooks() => Task.FromResult(_books.ToList());

    public Task<int> AddBook(Book book)
    {
        if (book.Id == 0)
        {
            book.Id = _nextId++;
        }

        _books.Add(book);
        return Task.FromResult(book.Id);
    }

    public Task DeleteBook(int id)
    {
        _books.RemoveAll(b => b.Id == id);
        return Task.CompletedTask;
    }
}
