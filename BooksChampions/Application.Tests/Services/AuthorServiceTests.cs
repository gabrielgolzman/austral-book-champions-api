using Application.Services;
using Application.Tests.Fakes;
using Domain.Entities;
using FluentAssertions;

namespace Application.Tests.Services;

[Trait("Category", "Unit")]
public class AuthorServiceTests
{
    private readonly FakeAuthorRepository _authorRepository = new();
    private readonly AuthorService _sut;

    public AuthorServiceTests()
    {
        _sut = new AuthorService(_authorRepository);
    }

    [Fact]
    public async Task GetAuthors_WhenAuthorsExist_ReturnsMappedDtos()
    {
        _authorRepository.Seed(
        [
            new Author { Id = 1, Name = "Jane Doe" },
            new Author { Id = 2, Name = "John Smith" }
        ]);

        var result = await _sut.GetAuthors();

        result.Should().HaveCount(2);
        result[0].Id.Should().Be(1);
        result[0].Name.Should().Be("Jane Doe");
        result[1].Id.Should().Be(2);
        result[1].Name.Should().Be("John Smith");
    }

    [Fact]
    public async Task GetAuthors_WhenNoAuthors_ReturnsEmptyList()
    {
        var result = await _sut.GetAuthors();

        result.Should().BeEmpty();
    }
}
