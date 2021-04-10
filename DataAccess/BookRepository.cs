using AspNetCoreApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreApp.DataAccess
{
    public class BookRepository:IBookRepository
    {
        private readonly BookContext _context;
        public BookRepository(BookContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books.ToList();
        }
        public IQueryable<Book> GetAllBooks()
        {
            return _context.Set<Book>();
        }

        public virtual async Task<ICollection<Book>> GetAllBooksAsync()
        {
            //return await _context.Books.ToListAsync();
            return await _context.Set<Book>().ToListAsync();
        }
        public virtual Book GetBookById(int bookId)
        {
            return _context.Books.Find(bookId);
        }

        public virtual async Task<Book> GetBookByIdAsync(int? bookId)
        {
            return await _context.Books.FindAsync(bookId);
        }
        public virtual void AddBook(Book bookEntity)
        {
            if (bookEntity != null)
            {
                _context.Books.Add(bookEntity);
            }
        }

        public virtual async Task AddBookAsync(Book bookEntity)
        {
            if (bookEntity != null)
            {
                await _context.Books.AddAsync(bookEntity);
            }
        }
        public virtual void UpdateBook(Book bookEntity)
        {
            if (bookEntity != null)
            {
                _context.Entry(bookEntity).State = EntityState.Modified;
            }
        }
        
        public virtual void DeleteBook(int bookId)
        {
            Book BookEntity = _context.Books.Find(bookId);
            _context.Books.Remove(BookEntity);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public virtual async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
