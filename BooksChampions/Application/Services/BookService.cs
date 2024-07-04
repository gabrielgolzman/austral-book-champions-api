using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepostory;
        public BookService(IBookRepository bookRepository) {
            _bookRepostory = bookRepository;
        }

        public List<BookDto> GetBooks() {
            
            var books =  _bookRepostory.GetBooks();

            return books
                .Select(book => new BookDto
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        Summary = book.Summary,
                        Rating = book.Rating,
                        PagesAmount = book.PagesAmount,
                        ImageUrl = book.ImageURL
                    })
                .ToList();
        }

        public int AddBook(BookDto bookDto)
        {
            return _bookRepostory.AddBook(new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Summary = bookDto.Summary,
                Rating = bookDto.Rating,
                PagesAmount = bookDto.PagesAmount,
                ImageURL = bookDto.ImageUrl
            });
        }

     
    }
}
