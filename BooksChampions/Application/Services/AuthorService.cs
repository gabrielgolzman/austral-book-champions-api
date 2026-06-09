using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository) {
            _authorRepository = authorRepository;
        }

        public async Task<List<AuthorDto>> GetAuthors() {
            
            List<Author> authors = await _authorRepository.GetAll();

            return authors
                .Select(author => new AuthorDto
                    {
                        Id = author.Id,
                        Name = author.Name,
                    })
                .ToList();
        }

        public async Task<AuthorDto> GetAuthor(int id)
        {
            Author? selected_author = await _authorRepository.Get(id);
            return new AuthorDto
            {
                Id = selected_author.Id,
                Name = selected_author.Name
            };
        }

        public void AddAuthor(AuthorDto author)
        {
            _authorRepository.Add(
                new Author
                {
                    Name = author.Name,
                }
            );
        }

        public async void UpdateAuthor(AuthorDto author)
        {
            Author? author_to_update = await _authorRepository.Get(author.Id);

            author_to_update.Name = author.Name;

            _authorRepository.Update(author_to_update);
        }

        public async void DeleteAuthor(int id)
        {
            Author? author_to_delete = await _authorRepository.Get(id);

            _authorRepository.Delete(author_to_delete);
        }

    }
}
