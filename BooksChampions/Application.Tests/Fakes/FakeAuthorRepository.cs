using Domain.Entities;
using Domain.Interfaces;

namespace Application.Tests.Fakes;

/// <summary>
/// Test fake: a simplified in-memory implementation of IAuthorRepository.
/// </summary>
public class FakeAuthorRepository : IAuthorRepository
{
    private readonly List<Author> _authors = [];
    private int _nextId = 1;

    public IReadOnlyList<Author> Authors => _authors;

    public void Seed(IEnumerable<Author> authors)
    {
        foreach (var author in authors)
        {
            _authors.Add(author);
            if (author.Id >= _nextId)
            {
                _nextId = author.Id + 1;
            }
        }
    }

    public Task<List<Author>> GetAll() => Task.FromResult(_authors.ToList());

    public Task<Author> Get<TId>(TId id)
    {
        if (id is not int authorId)
        {
            throw new ArgumentException("Author id must be an integer.", nameof(id));
        }

        var author = _authors.First(a => a.Id == authorId);
        return Task.FromResult(author);
    }

    public void Add(Author item)
    {
        if (item.Id == 0)
        {
            item.Id = _nextId++;
        }

        _authors.Add(item);
    }

    public void Update(Author item)
    {
        var index = _authors.FindIndex(a => a.Id == item.Id);
        if (index >= 0)
        {
            _authors[index] = item;
        }
    }

    public void Delete(Author item) => _authors.RemoveAll(a => a.Id == item.Id);

    public Task<List<Author>> GetAuthorsByIds(List<int> authorIds) =>
        Task.FromResult(_authors.Where(a => authorIds.Contains(a.Id)).ToList());
}
