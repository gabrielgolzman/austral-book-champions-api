namespace Application.Models
{
    public class BookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Summary { get; set; }
        public int Rating { get; set; }
        public int PagesAmount { get; set; }
        public string? ImageUrl { get; set; }
        public List<AuthorDto> Authors { get; set; }
    }
}
