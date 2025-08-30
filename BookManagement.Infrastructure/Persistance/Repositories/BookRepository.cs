using BookManagement.Application.Persistance.Repositories;
using BookManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Infrastructure.Persistance.Repositories
{
    public class BookRepository :RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) :base(context) { }
       
    }
}
