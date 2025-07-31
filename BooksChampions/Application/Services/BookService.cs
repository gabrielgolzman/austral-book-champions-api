using Application.Models;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository) {
            _bookRepository = bookRepository;
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
                        Authors = book.Authors.Select(x => new AuthorDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                        })
                        .ToList()
                    })
                .ToList();
        }

        public Task<int> AddBook(BookDto bookDto)
        {
            return _bookRepository.AddBook(new Book
            {
                Title = bookDto.Title,
                Summary = bookDto.Summary,
                Rating = bookDto.Rating,
                PagesAmount = bookDto.PagesAmount,
                ImageURL = bookDto.ImageUrl
            });
        }

        public void DeleteBook(int id)
        {
             _bookRepository.DeleteBook(id);
        }

     
    }
}
