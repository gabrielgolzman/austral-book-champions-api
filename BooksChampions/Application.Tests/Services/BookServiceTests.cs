using Application.Models;
using Application.Services;
using Application.Tests.Fakes;
using Domain.Entities;
using FluentAssertions;

namespace Application.Tests.Services;

[Trait("Category", "Unit")]
public class BookServiceTests
{
    private readonly FakeBookRepository _bookRepository = new();
    private readonly FakeAuthorRepository _authorRepository = new();
    private readonly BookService _sut;

    public BookServiceTests()
    {
        _sut = new BookService(_bookRepository, _authorRepository);
    }

    [Fact]
    public async Task GetBooks_WhenBooksExist_ReturnsMappedDtos()
    {
        _bookRepository.Seed(
        [
            new Book
            {
                Id = 1,
                Title = "Test Book",
                Summary = "Summary",
                Rating = 5,
                PagesAmount = 100,
                ImageURL = "http://image.url",
                IsAvailable = true,
                Authors = [new Author { Id = 10, Name = "Author One" }]
            }
        ]);

        var result = await _sut.GetBooks();

        result.Should().HaveCount(1);
        result[0].Id.Should().Be(1);
        result[0].Title.Should().Be("Test Book");
        result[0].Summary.Should().Be("Summary");
        result[0].Rating.Should().Be(5);
        result[0].PagesAmount.Should().Be(100);
        result[0].ImageUrl.Should().Be("http://image.url");
        result[0].IsAvailable.Should().BeTrue();
        result[0].Authors.Should().ContainSingle(a => a.Id == 10 && a.Name == "Author One");
    }

    [Fact]
    public async Task GetBooks_WhenNoBooks_ReturnsEmptyList()
    {
        var result = await _sut.GetBooks();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddBook_WithoutAuthorIds_CreatesBookWithoutAuthors()
    {
        var bookDto = new BookDto
        {
            Title = "New Book",
            Summary = "A summary",
            Rating = 4,
            PagesAmount = 200,
            ImageUrl = "http://cover.url",
            IsAvailable = true
        };

        var result = await _sut.AddBook(bookDto);

        result.Should().Be(1);
        _bookRepository.Books.Should().ContainSingle(b =>
            b.Title == "New Book" &&
            b.Summary == "A summary" &&
            b.Rating == 4 &&
            b.PagesAmount == 200 &&
            b.ImageURL == "http://cover.url" &&
            b.IsAvailable &&
            b.Authors.Count == 0);
    }

    [Fact]
    public async Task AddBook_WithAuthorIds_LinksAuthors()
    {
        _authorRepository.Seed(
        [
            new Author { Id = 1, Name = "Author A" },
            new Author { Id = 2, Name = "Author B" }
        ]);
        var bookDto = new BookDto
        {
            Title = "Linked Book",
            Rating = 3,
            PagesAmount = 150,
            IsAvailable = true,
            AuthorIds = [1, 2]
        };

        var result = await _sut.AddBook(bookDto);

        result.Should().Be(1);
        var savedBook = _bookRepository.Books.Should().ContainSingle().Subject;
        savedBook.Authors.Should().HaveCount(2);
        savedBook.Authors.Should().Contain(a => a.Id == 1 && a.Name == "Author A");
        savedBook.Authors.Should().Contain(a => a.Id == 2 && a.Name == "Author B");
    }

    [Fact]
    public async Task DeleteBook_RemovesBookFromFake()
    {
        _bookRepository.Seed(
        [
            new Book
            {
                Id = 5,
                Title = "Book To Delete",
                Rating = 3,
                PagesAmount = 100,
                IsAvailable = true
            }
        ]);

        await _sut.DeleteBook(5);

        _bookRepository.Books.Should().BeEmpty();
    }
}
