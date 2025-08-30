using BookManagement.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Application.IServices
{
    public interface IBookService
    {
        Task<DataListResponseModel> GetAllBooksAsync(DataListRequestModel requestModel);
        Task<string> SaveBookAsync(BookModel model);
        Task<BookDetailModel> GetBookByIdAsync(Guid bookId);
        Task<string> UpdateBookAsync(Guid bookId, BookModel updateModel);
        Task<string> DeleteAsync(Guid bookId);
       
    }
}
