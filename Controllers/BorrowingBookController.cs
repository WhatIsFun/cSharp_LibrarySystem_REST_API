using cSharp_LibrarySystemWebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace cSharp_LibrarySystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingBookController : ControllerBase
    {
        public static LibraryDbContext _context;
        public BorrowingBookController(LibraryDbContext DB)
        {
            _context = DB;
        }

        [Authorize]
        [HttpPost("CreateBorrowingTransaction")]
        public IActionResult CreateBorrowingTransaction(int patronId, int bookId)
        {
            try
            {
                // Check if the book is available for borrowing
                var book = _context.Book.FirstOrDefault(b => b.BookId == bookId);
                if (book == null || !book.IsAvailable)
                {
                    return BadRequest("The selected book is not available for borrowing.");
                }

                // Check if the patron exists
                var patron = _context.Patron.FirstOrDefault(p => p.PatronId == patronId);
                if (patron == null)
                {
                    return BadRequest("Patron not found.");
                }
                var transaction = new BorrowingTransaction
                {
                    PatronId = patronId,
                    BookId = bookId,
                    BorrowDate = DateTime.Now,
                    ReturnDate = null
                };
                _context.BorrowingTransaction.Add(transaction);

                ToggleBookAvailability(book);

                _context.SaveChanges();
                return Ok("Borrowing successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("ReturnBook")]
        public IActionResult MarkBookAsReturned(int returnBookId)
        {
            try
            {
                var transaction = _context.BorrowingTransaction.FirstOrDefault(bt => bt.BookId == returnBookId);

                if (transaction == null)
                {
                    return BadRequest("Borrowing transaction not found.");
                }

                if (transaction.ReturnDate != null)
                {
                    return BadRequest("The book has already been returned.");
                }

                transaction.ReturnDate = DateTime.Now;

                var book = _context.Book.FirstOrDefault(b => b.BookId == transaction.BookId);
                if (book != null)
                {
                    ToggleBookAvailability(book);
                }

                _context.SaveChanges();
                return Ok("Book marked as returned successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("ByPatronId")]
        public IActionResult GetPatronBorrowingHistory(int patronID)
        {
            try
            {
                var patron = _context.Patron
                    .Include(p => p.BorrowingTransactions)
                    .ThenInclude(bt => bt.Book) // Include the Book entity
                    .FirstOrDefault(p => p.PatronId == patronID);

                if (patron == null)
                {
                    return NotFound("No patron found with this ID.");
                }

                var borrowingHistory = patron.BorrowingTransactions.OrderByDescending(bt => bt.BorrowDate).ToList();

                if (borrowingHistory.Count > 0)
                {
                    return Ok(borrowingHistory);
                }
                else
                {
                    return NotFound("No borrowing history found for this patron.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("AllHistory")]
        public IActionResult BorrowingHistory()
        {
            try
            {
                var transactions = _context.BorrowingTransaction.Include(p => p.Patron).Include(b => b.Book).ToList();

                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound("No transactions found.");
                }
                var patron = transactions.First().Patron;
                var historyList = transactions.Select(history => new
                {
                    BookTitle = history.Book.Title,
                    BorrowDate = history.BorrowDate,
                    ReturnDate = history.ReturnDate ?? DateTime.MinValue
                }).ToList();

                var result = new
                {
                    PatronName = patron.Name,
                    PatronID = patron.PatronId,
                    PatronPhone = patron.PhoneNum,
                    BorrowingHistory = historyList
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpPut("Flag")]
        private bool ToggleBookAvailability(Book book)
        {
            bool isAvailable = book.IsAvailable;
            book.IsAvailable = !isAvailable;
            return isAvailable;
        }



    }
}
