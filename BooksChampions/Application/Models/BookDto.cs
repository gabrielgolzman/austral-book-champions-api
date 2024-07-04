using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class BookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public string? Summary { get; set; }
        public int Rating { get; set; }
        public int PagesAmount { get; set; }
        public string? ImageUrl { get; set; }
    }
}
