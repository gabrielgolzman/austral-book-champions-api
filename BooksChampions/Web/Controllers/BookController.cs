using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService) {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult Get() {
            try
            {
                return Ok(_bookService.GetBooks());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "API is running", timestamp = DateTime.UtcNow });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(BookDto bookDto)
        {
            return Ok(_bookService.AddBook(bookDto));
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            _bookService.DeleteBook(id);
            return Ok();
        }

    }
}
