using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService) {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult Get() {

            return Ok(_bookService.GetBooks());
        }

        [HttpPost]
        public IActionResult Post(BookDto bookDto)
        {
            return Ok(_bookService.AddBook(bookDto));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _bookService.DeleteBook(id);
            return Ok();
        }

    }
}
