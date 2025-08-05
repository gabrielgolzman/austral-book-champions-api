using Application.Models;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository) {
            _authorRepository = authorRepository;
        }

        public async Task<List<AuthorDto>> GetAuthors() {
            
            var authors = _authorRepository.Get();

            return authors
                .Select(author => new AuthorDto
                    {
                        Id = author.Id,
                        Name = author.Name,
                    })
                .ToList();
        }

     
    }
}
