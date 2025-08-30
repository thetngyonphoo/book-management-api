using BookManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Application.Persistance.Repositories
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
    }
}
