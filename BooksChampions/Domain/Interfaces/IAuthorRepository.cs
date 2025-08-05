using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAuthorRepository: IBaseRepository<Author>
    {
        public Task<List<Author>> GetAuthorsByIds(List<int> authorIds);
    }
}
