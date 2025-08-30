using BookManagement.Application.Persistance;
using BookManagement.Application.Persistance.Repositories;
using BookManagement.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Infrastructure.Persistance
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _appDbContext;

        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            this.bookRepository = new BookRepository(appDbContext);
        }
        public IBookRepository bookRepository { get; set; }

        public Task<int> SaveChangeAsync()
        {
            var entries = this._appDbContext.ChangeTracker.Entries()
                .Where(x=> (x.State == EntityState.Added || x.State == EntityState.Modified)).ToList();

            foreach (var entry in entries) {
                Type type =entry.Entity.GetType();
                if (entry.State == EntityState.Added)
                {
                    PropertyInfo createby = type.GetProperty("CreatedBy")!;
                    if (createby is not null)
                        createby.SetValue(entry.Entity, "SystemUser",null);
                    PropertyInfo createat = type.GetProperty("CreatedDate")!;
                    if (createat is not null)
                        createat.SetValue(entry.Entity, DateTime.Now, null);
                }
                else
                {
                    PropertyInfo modifiedby = type.GetProperty("ModifiedBy")!;
                    if (modifiedby is not null)
                        modifiedby.SetValue(entry.Entity, "SystemUser",null);
                    PropertyInfo modifiedat = type.GetProperty("ModifiedDate")!;
                    if (modifiedat is not null)
                        modifiedat.SetValue(entry.Entity, DateTime.Now, null);
                }
            }
            return this._appDbContext.SaveChangesAsync();
        }
    }
}
