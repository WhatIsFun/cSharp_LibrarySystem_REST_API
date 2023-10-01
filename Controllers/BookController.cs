using cSharp_LibrarySystemWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cSharp_LibrarySystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public static LibraryDbContext _context;
        public BookController(LibraryDbContext DB)
        {
            _context = DB;
        }
        [HttpPost]
        public void addBook(Book book, string title, string author, int publicationYear)
        {
            var newBook = new Book
            {
                Title = title,
                Author = author,
                PublicationYear = publicationYear,
                IsAvailable = true // Assuming the book is available when added
            };
            _context.Book.Add(book);
            _context.SaveChanges();
        }
        [HttpGet]
        public List<Book> getAllBooks()
        {
            return _context.Book.ToList();
        }
        [HttpGet("getById")]
        public Book getBookById(int searchBookId)
        {
            return _context.Book.FirstOrDefault(b => b.BookId == searchBookId);
        }
        [HttpPut]
        public void updateBook(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            _context.SaveChanges();
        }
        [HttpDelete]
        public void deleteBook(int bookId)
        {
            var book = _context.Book.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                _context.Book.Remove(book);
                _context.SaveChanges();
            }
        }
        [HttpGet("byPublicationYear/{year}")]
        public void GetBooksByPublicationYear(int year)
        {
            var books = _context.Book.Where(b => b.PublicationYear == year).ToList();
        }

        [HttpGet("countByPublicationYear/{year}")]
        public void GetBookCountByPublicationYear(int year)
        {
            var count = _context.Book.Count(b => b.PublicationYear == year);
        }

        [HttpGet("available")]
        public void GetAvailableBooks()
        {
            var availableBooks = _context.Book.Where(b => b.IsAvailable).ToList();
        }

        [HttpGet("byAuthor/{author}")]
        public void GetBooksByAuthor(string author)
        {
            var booksByAuthor = _context.Book.Where(b => b.Author == author).ToList();
        }

        [HttpGet("byTitle/{title}")]
        public void GetBookByTitle(string title)
        {
            var book = _context.Book.FirstOrDefault(b => b.Title == title);
        }
        [HttpGet("byBorrowingDate/{borrowDate}")]
        public void GetTransactionsByBorrowingDate(DateTime borrowDate)
        {
            var transactionsByBorrowDate = _context.BorrowingTransaction
                .Where(bt => bt.BorrowDate.Date == borrowDate.Date)
                .ToList();
        }
        [HttpGet("byReturnDate/{returnDate}")]
        public void GetTransactionsByReturnDate(DateTime returnDate)
        {
            var transactionsByReturnDate = _context.BorrowingTransaction
                .Where(bt => bt.ReturnDate.HasValue && bt.ReturnDate.Value.Date == returnDate.Date)
                .ToList();
        }
        [HttpGet("byBookId/{bookId}")]
        public void GetTransactionsByBook(int bookId)
        {
            var transactionsByBook = _context.BorrowingTransaction
                .Where(bt => bt.BookId == bookId)
                .ToList();
        }
        [HttpGet("bookBorrowCount/{bookId}")]
        public void GetBookBorrowCount(int bookId)
        {
            var borrowCount = _context.BorrowingTransaction
                .Count(bt => bt.BookId == bookId);
        }

    }
}
