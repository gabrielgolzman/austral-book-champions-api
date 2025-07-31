using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAuthorRepository
    {
        public Task<List<Author>> GetAuthors();
    }
}
