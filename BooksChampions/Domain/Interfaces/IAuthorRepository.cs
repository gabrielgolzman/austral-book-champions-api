using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAuthorRepository
    {
        public Task<List<Author>> GetAuthors();
        public Task<List<Author>> GetAuthorsByIds(List<int> authorIds);
    }
}
