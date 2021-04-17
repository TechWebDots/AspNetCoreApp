using AspNetCoreApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            try
            {
                //return await _context.Books.ToListAsync();
                return await _context.Set<Book>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }            
            
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
            if (bookEntity == null)
            {
                throw new ArgumentNullException($"{nameof(AddBookAsync)} entity must not be null");
            }
            try
            {
                if (bookEntity != null)
                {
                    await _context.Books.AddAsync(bookEntity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(bookEntity)} could not be saved: {ex.Message}");
            }            
        }
        public virtual void UpdateBook(Book bookEntity)
        {
            if (bookEntity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateBook)} entity must not be null");
            }

            try
            {
                if (bookEntity != null)
                {
                    ShowEntityState(_context);
                    _context.Entry(bookEntity).State = EntityState.Modified;
                    ShowEntityState(_context);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(bookEntity)} state could not be updated: {ex.Message}");
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
            int records = 0;
            IDbContextTransaction tx = null;
            try
            {
                using (tx = await _context.Database.BeginTransactionAsync())
                {
                    records = await _context.SaveChangesAsync();
                    tx.Commit();
                    return records;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Book)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            // TODO: decide which value should be written to database
                            // proposedValues[property] = <value to be saved>;
                        }

                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                SqlException s = ex.InnerException as SqlException;
                var errorMessage = $"{ex.Message}" + " {ex?.InnerException.Message}" + " rolling back…";
                tx.Rollback();
            }
            return records;
        }
        public static void ShowEntityState(BookContext context)
        {
            foreach (EntityEntry entry in context.ChangeTracker.Entries())
            {
                //Discards are local variables which you can assign but cannot read from. i.e. they are “write-only” local variables.
                _ = ($"type: {entry.Entity.GetType().Name}, state: {entry.State}," +$" {entry.Entity}");
            }            
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
