using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repository;
using Infrastructure.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repository;

[Trait("Category", "Integration")]
public class BookRepositoryTests : IDisposable
{
    private readonly Infrastructure.BookDbContext _dbContext;
    private readonly BookRepository _sut;

    public BookRepositoryTests()
    {
        _dbContext = DbContextFactory.Create();
        _sut = new BookRepository(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    [Fact]
    public async Task GetBooks_IncludesAuthors()
    {
        var author = new Author { Name = "Included Author" };
        var book = new Book
        {
            Title = "Book With Author",
            Rating = 5,
            PagesAmount = 100,
            IsAvailable = true,
            Authors = [author]
        };
        _dbContext.Authors.Add(author);
        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();

        var result = await _sut.GetBooks();

        result.Should().ContainSingle(b => b.Title == "Book With Author");
        result[0].Authors.Should().ContainSingle(a => a.Name == "Included Author");
    }

    [Fact]
    public async Task AddBook_WithExistingAuthors_AttachesAsUnchanged()
    {
        var author = new Author { Name = "Existing Author" };
        _dbContext.Authors.Add(author);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();

        var book = new Book
        {
            Title = "New Book",
            Rating = 4,
            PagesAmount = 50,
            IsAvailable = true,
            Authors = [author]
        };

        await _sut.AddBook(book);

        (await _dbContext.Authors.CountAsync()).Should().Be(1);
        var savedBook = await _dbContext.Books.Include(b => b.Authors).SingleAsync();
        savedBook.Authors.Should().ContainSingle(a => a.Name == "Existing Author");
    }

    [Fact]
    public async Task DeleteBook_WhenExists_RemovesBook()
    {
        var book = new Book
        {
            Title = "To Delete",
            Rating = 3,
            PagesAmount = 80,
            IsAvailable = true
        };
        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();

        await _sut.DeleteBook(book.Id);

        (await _dbContext.Books.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task DeleteBook_WhenNotExists_DoesNotThrow()
    {
        var act = async () => await _sut.DeleteBook(999);

        await act.Should().NotThrowAsync();
    }
}
