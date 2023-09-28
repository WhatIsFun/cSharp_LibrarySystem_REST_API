using cSharp_LibrarySystemWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpPost]
        public void CreateBorrowingTransaction(int patronId, int bookId)
        {
            // Check if the book is available for borrowing
            var book = _context.Book.FirstOrDefault(b => b.BookId == bookId);
            if (book == null || !book.IsAvailable)
            {
                throw new Exception("The selected book is not available for borrowing.");
            }

            // Check if the patron exists
            var patron = _context.Patron.FirstOrDefault(p => p.PatronId == patronId);
            if (patron == null)
            {
                throw new Exception("Patron not found.");
            }

            var transaction = new BorrowingTransaction
            {
                PatronId = patronId,
                BookId = bookId,
                BorrowDate = DateTime.Now,
                ReturnDate = null
            };
            _context.BorrowingTransaction.Add(transaction);

            book.IsAvailable = false;

            _context.SaveChanges();
            Console.WriteLine("Borrowing successfully");
        }

        public void MarkBookAsReturned(int returnBookId)
        {
            var transaction = _context.BorrowingTransaction.FirstOrDefault(bt => bt.BookId == returnBookId);

            if (transaction == null)
            {
                Console.WriteLine("Borrowing transaction not found.");
                return;
            }

            if (transaction.ReturnDate != null)
            {
                Console.WriteLine("The book has already been returned.");
                return;
            }

            transaction.ReturnDate = DateTime.Now;

            var book = _context.Book.FirstOrDefault(b => b.BookId == transaction.BookId);
            if (book != null)
            {
                book.IsAvailable = true;
            }

            _context.SaveChanges();
        }
        [HttpGet("ByPatronId")]
        public void GetPatronBorrowingHistory(int patronID)
        {
            var patron = _context.Patron.Include(p => p.BorrowingTransactions)
                            .FirstOrDefault(p => p.PatronId == patronID);
            if (patron == null)
            {
                Console.WriteLine("No history found for this Patron");
            }

            var borrowingHistory = patron.BorrowingTransactions.OrderByDescending(bt => bt.BorrowDate).ToList();
            if (borrowingHistory.Count == 0)
            {
                Console.WriteLine("No borrowing history found for this patron.");
            }
            else
            {
                foreach (var transaction in borrowingHistory)
                {
                    Console.WriteLine($"Borrowing Transaction ID: {transaction.BorrowingTransactionId}\nPatron ID:{transaction.Patron.PatronId}\nPatron Name: {transaction.Patron.Name}\nPatron Phone Number: {transaction.Patron.PhoneNum}\nBook ID: {transaction.BookId}\nBook Title: {transaction.Book.Title}\nBorrow Date: {transaction.BorrowDate}\nReturn Date: {transaction.ReturnDate}\n____________________");
                }
            }
        }
        [HttpGet("AllHistory")]
        public void BorrowingHistory()
        {
            var transaction = _context.BorrowingTransaction.Include(p => p.Patron).Include(b => b.Book);
            if (transaction == null)
            {
                Console.WriteLine("No transaction found");
            }
            else
            {
                foreach (var tran in transaction)
                {
                    Console.WriteLine($"Borrowing Transaction ID: {tran.BorrowingTransactionId}\nPatron ID:{tran.Patron.PatronId}\nPatron Name: {tran.Patron.Name}\nPatron Phone Number: {tran.Patron.PhoneNum}\nBook ID: {tran.BookId}\nBook Title: {tran.Book.Title}\nBorrow Date: {tran.BorrowDate}\nReturn Date: {tran.ReturnDate}\n____________________");
                }
            }
        }

    }
}
