using BookManagement.Application.Dto;
using BookManagement.Application.IServices;
using BookManagement.Application.Persistance;
using BookManagement.Domain.Entities;
using BookManagement.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IBookService _bookService;
        public BookController(IRepositoryWrapper wrapper, IBookService bookService)
        {
            _wrapper = wrapper;
            _bookService = bookService;
        }

        [HttpPost("GetAllBooks")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ApiResponseModel<IEnumerable<DataListResponseModel>>>> GetAllBooks(DataListRequestModel requestModel)
        {
            var resp = new DataListResponseModel();
            
            resp = await _bookService.GetAllBooksAsync(requestModel);

            return Ok(new ApiResponseModel<DataListResponseModel>
            {              
                Code = 200,
                Message = "Success",
                Data = resp
            });
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseModel<object>>> SaveBook(BookModel model)
        {
            var resp = await _bookService.SaveBookAsync(model);

            return Ok(new ApiResponseModel<object>
            {
                Code = 200,
                Message = "Success.",
                Data = resp
            });
        }

        [HttpGet("{bookId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseModel<BookDetailModel>>> GetBookById(Guid bookId)
        {
            var resp = await _bookService.GetBookByIdAsync(bookId);
            if (resp == null)
                return NotFound(ApiResponseModel<string>.Fail("Book not found"));

            return Ok(new ApiResponseModel<BookDetailModel>
            {
                Code = 200,
                Message = "Success.",
                Data = resp
            });
        }

        [HttpPut("{bookId:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseModel<object>>> UpdateBook(Guid bookId, BookModel updateModel)
        {
            var resp = await _bookService.UpdateBookAsync(bookId, updateModel);

            return Ok(new ApiResponseModel<object>
            {
                Code = 200,
                Message = "Success.",
                Data = resp
            });
        }

        [HttpDelete("{bookId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseModel<object>>> DeleteBook(Guid bookId)
        {
            var resp = await _bookService.DeleteAsync(bookId);

            return Ok(new ApiResponseModel<object>
            {
                Code = 200,
                Message = "Success.",
                Data = resp
            });
        }
    }
}
