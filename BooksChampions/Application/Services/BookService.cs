using Application.Models;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository) {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<List<BookDto>> GetBooks() {
            
            var books =  await _bookRepository.GetBooks();

            return books
                .Select(book => new BookDto
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Summary = book.Summary,
                        Rating = book.Rating,
                        PagesAmount = book.PagesAmount,
                        ImageUrl = book.ImageURL,
                        IsAvailable = book.IsAvailable,
                        Authors = book.Authors.Select(x => new AuthorDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                        })
                        .ToList()
                    })
                .ToList();
        }

        public async Task<int> AddBook(BookDto bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Summary = bookDto.Summary,
                Rating = bookDto.Rating,
                PagesAmount = bookDto.PagesAmount,
                ImageURL = bookDto.ImageUrl,
                IsAvailable = bookDto.IsAvailable
            };

            if (bookDto.AuthorIds != null && bookDto.AuthorIds.Any())
            {
                var authors = await _authorRepository.GetAuthorsByIds(bookDto.AuthorIds);
                book.Authors = authors;
            }

            return await _bookRepository.AddBook(book);
        }

        public void DeleteBook(int id)
        {
             _bookRepository.DeleteBook(id);
        }

     
    }
}
