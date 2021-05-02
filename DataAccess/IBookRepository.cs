using AspNetCoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApp.DataAccess
{
    public interface IBookRepository : IDisposable
    {
        IEnumerable<Book> GetAll();
        IQueryable<Book> GetAllBooks();
        Task<ICollection<Book>> GetAllBooksAsync();
        Book GetBookById(int bookId);
        Task<Book> GetBookByIdAsync(int? bookId);
        void AddBook(Book bookEntity);
        Task AddBookAsync(Book bookEntity);
        void UpdateBook(Book bookEntity);
        void DeleteBook(int bookId);
        void Save();
        Task<int> SaveAsync(CancellationToken cancellationtoken);
    }
}
