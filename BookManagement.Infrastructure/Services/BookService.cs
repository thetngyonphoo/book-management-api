using BookManagement.Application.Dto;
using BookManagement.Application.IServices;
using BookManagement.Application.Persistance;
using BookManagement.Domain.Constants;
using BookManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<BookService> _logger;
        public BookService(IRepositoryWrapper repository, ILogger<BookService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DataListResponseModel> GetAllBooksAsync(DataListRequestModel requestModel)
        {
            try
            {
                var result = new DataListResponseModel();
                DateTime? fromDate = null;
                DateTime? toDate = null;

                if (!string.IsNullOrWhiteSpace(requestModel.param?.fromDate) &&
                DateTime.TryParseExact(requestModel.param.fromDate, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var from))
                {
                    fromDate = from;
                }

                if (!string.IsNullOrWhiteSpace(requestModel.param?.toDate) &&
                DateTime.TryParseExact(requestModel.param.toDate, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var to))
                {
                    toDate = to;
                }

                var query = _repository.bookRepository.GetByCondition(x => x.IsDelete == false);

                if (fromDate.HasValue && toDate.HasValue)
                {                   
                    var endOfDay = toDate.Value.Date.AddDays(1).AddTicks(-1);

                    query = query.Where(x => x.CreatedDate >= fromDate.Value
                                          && x.CreatedDate <= endOfDay);
                }

                var books = await query.OrderByDescending(x => x.CreatedDate)
                          .Skip(requestModel.start)
                          .Take(requestModel.length)
                          .ToListAsync();

                var bookDetails = books.Select(x => new BookDetailModel
                {
                    BookId = x.BookId,
                    Title = x.Title,
                    Author = x.Author,
                    PublishedDate = x.PublishedDate,
                    CategoryName = x.Category.ToString(),
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                }).ToList();

                result = new DataListResponseModel()
                {
                    draw = requestModel.draw,
                    recordsTotal = _repository.bookRepository.GetByCondition(x => x.IsDelete == false).Count(),
                    recordsFiltered = books.Count(),
                    data = bookDetails
                };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving book.");
                throw;
            }
        }
        public async Task<string> SaveBookAsync(BookModel model)
        {
            if (model == null)
            {
                _logger.LogWarning("BookModel is null.");
                throw new ArgumentNullException(nameof(model), "BookModel cannot be null.");
            }
            try
            {
                bool parsed = Enum.TryParse<BookCategory>(model.CategoryName, ignoreCase: true, out var category);
                if (!parsed || !Enum.IsDefined(typeof(BookCategory), category) || category == BookCategory.Other)
                {
                    category = BookCategory.Other;
                }

                Book bk = new Book
                {
                    BookId = Guid.NewGuid(),
                    Title = model.Title,
                    Author = model.Author,
                    PublishedDate = model.PublishedDate,
                    Category = category,
                    IsDelete = false
                };
                await _repository.bookRepository.AddAsync(bk);
                await _repository.SaveChangeAsync();
                return bk.BookId.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while saving book '{Title}'.", model.Title);
                throw;
            }
        }

        public async Task<BookDetailModel> GetBookByIdAsync(Guid bookId)
        {
            try
            {
                var bookEntity = await _repository.bookRepository.GetByCondition(x => x.IsDelete == false && x.BookId == bookId).FirstOrDefaultAsync();

                if (bookEntity == null)
                {
                    _logger.LogWarning("No book found with Id: {BookId}", bookId);
                    return null!;
                }
                ;

                var book = new BookDetailModel
                {
                    BookId = bookEntity.BookId,
                    Title = bookEntity.Title,
                    Author = bookEntity.Author,
                    PublishedDate = bookEntity.PublishedDate,
                    CategoryName = Enum.GetName(typeof(BookCategory), bookEntity.Category),
                    CreatedBy = bookEntity.CreatedBy,
                    CreatedDate = bookEntity.CreatedDate,
                    ModifiedBy = bookEntity.ModifiedBy,
                    ModifiedDate = bookEntity.ModifiedDate,
                };

                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving book with Id: {BookId}", bookId);
                throw;
            }
        }

        public async Task<string> UpdateBookAsync(Guid bookId, BookModel updateModel)
        {
            try
            {
                var existingBook = await _repository.bookRepository.GetByCondition(x => x.IsDelete == false && x.BookId == bookId).FirstOrDefaultAsync();

                if (existingBook == null)
                {
                    _logger.LogWarning("No book found with Id: {BookId}", bookId);
                    return null!;
                }

                bool parsed = Enum.TryParse<BookCategory>(updateModel.CategoryName, ignoreCase: true, out var category);
                if (!parsed || !Enum.IsDefined(typeof(BookCategory), category) || category == BookCategory.Other)
                {
                    category = BookCategory.Other;
                }

                existingBook.Title = updateModel.Title;
                existingBook.Author = updateModel.Author;
                existingBook.PublishedDate = updateModel.PublishedDate;
                existingBook.Category = category;
                existingBook.IsDelete = false;
                _repository.bookRepository.Update(existingBook);
                await _repository.SaveChangeAsync();
                return existingBook.BookId.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating book with Id: {BookId}", bookId);
                throw;
            }
        }

        public async Task<string> DeleteAsync(Guid bookId)
        {
            try
            {
                var bookEntity = await _repository.bookRepository.GetByCondition(x => x.IsDelete == false && x.BookId == bookId).FirstOrDefaultAsync();

                if (bookEntity == null)
                {
                    _logger.LogWarning("No book found with Id: {BookId}", bookId);
                    return null!;
                }

                bookEntity.IsDelete = true;
                await _repository.SaveChangeAsync();
                return bookEntity.BookId.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting book with Id: {BookId}", bookId);
                throw;
            }
        }

    }
}
