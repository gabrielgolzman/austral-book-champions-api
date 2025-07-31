using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Summary { get; set; }
        public int Rating { get; set; }
        public int PagesAmount { get; set; }
        public string? ImageURL { get; set; }
        public List<Author> Authors { get; set; } = new List<Author>();
    }
}
