using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBookRepository
    {
        public List<Book> GetBooks();

        public int AddBook(Book book);

        public void DeleteBook(int id);
    }
}
