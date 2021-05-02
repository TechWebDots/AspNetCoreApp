using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCoreApp.DataAccess;
using AspNetCoreApp.Models;
using MongoDB.Driver;
using Microsoft.Data.SqlClient;
using System.Threading;

namespace AspNetCoreApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        // Define the cancellation token.
        CancellationTokenSource _cts=null ;
        public BooksController(IBookRepository bookRepository)
        {
            #region MyRegion
            //MongoCRUD db = new MongoCRUD("Books");
            //var record = db.GetBookById("Book",new Guid("c16d4bf3-8328-4563-91b6-4a501c0329c2"));
            #endregion
            _bookRepository = bookRepository;            
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _bookRepository.GetAllBooksAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,Publisher")] Book book)
        {
            _cts = new CancellationTokenSource();
            // send a cancel after 4000 ms or call cts.Cancel();
            _cts.CancelAfter(4000);
            CancellationToken ct = _cts.Token;

            if (ModelState.IsValid)
            {
                await _bookRepository.AddBookAsync(book);
                await _bookRepository.SaveAsync(ct);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Publisher,RowVersion")] Book book)
        {
            //CancellationToken disconnectedToken = Response.ClientDisconnectedToken;
            //var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, disconnectedToken);
            _cts = new CancellationTokenSource();
            // send a cancel after 4000 ms or call cts.Cancel();
            _cts.CancelAfter(4000); 
            CancellationToken ct = _cts.Token;

            if (id != book.BookId || !BookExists(book.BookId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bookRepository.UpdateBook(book);
                    await _bookRepository.SaveAsync(ct);                     
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. The Book details was updated by another user, Please reload to get the latest record!");
                    
                    return View(book);
                }
                catch (OperationCanceledException ex)
                {                   
                    ModelState.AddModelError(string.Empty, "An error occured - " + ex.Message);
                    return View(book);
                }
                catch (Exception ex)
                {
                    SqlException s = ex.InnerException as SqlException;
                    if (s != null && s.Number == 2627)
                    {
                        ModelState.AddModelError(string.Empty, string.Format("Book Title '{0}' already exists.", book.Title));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An error occured - please contact your system administrator.");
                    }
                    return View(book);
                }
                finally
                {
                    _cts.Dispose();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _cts = new CancellationTokenSource();
            // send a cancel after 4000 ms or call cts.Cancel();
            _cts.CancelAfter(4000);
            CancellationToken ct = _cts.Token;

            var book = await _bookRepository.GetBookByIdAsync(id);
            _bookRepository.DeleteBook(id);
            await _bookRepository.SaveAsync(ct);
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _bookRepository.GetAllBooks().Any(e => e.BookId == id);
        }
    }
}
