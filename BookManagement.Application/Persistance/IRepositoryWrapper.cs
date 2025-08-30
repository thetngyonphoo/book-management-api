using BookManagement.Application.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Application.Persistance
{
    public interface IRepositoryWrapper
    {
        IBookRepository bookRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
