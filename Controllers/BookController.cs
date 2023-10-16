using cSharp_LibrarySystemWebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

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

        [Authorize]
        [HttpPost("addBook")]
        public IActionResult AddBook(Book book)
        {
            try
            {
                _context.Book.Add(book);
                _context.SaveChanges();
                return Ok("Book has been added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add the book: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("getAllBooks")]
        public IActionResult GetAllBooks()
        {
            try
            {
                var allBooks = _context.Book.ToList();
                if (allBooks != null) 
                {
                    return Ok(allBooks);
                }
                return NotFound("There is no books found");
            }
            catch { return BadRequest("Error ..."); }
        }

        [Authorize]
        [HttpGet("getBookById")]
        public IActionResult GetBookById(int searchBookId)
        {
            try
            {
                var book = _context.Book.FirstOrDefault(b => b.BookId == searchBookId);
                if (book == null)
                {
                    return NotFound("Book not found.");
                }
                return Ok(book);
            }catch { return BadRequest("Error ..."); }
            
        }

        [Authorize]
        [HttpPut("updateBook")]
        public IActionResult UpdateBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Invalid book data.");
            }

            try
            {
                _context.Entry(book).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok("Book has been updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update the book: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("deleteBook/{bookId}")]
        public IActionResult DeleteBook(int bookId)
        {
            try
            {
                var book = _context.Book.FirstOrDefault(b => b.BookId == bookId);
                if (book == null)
                {
                    return NotFound("Book not found.");
                }

                try
                {
                    _context.Book.Remove(book);
                    _context.SaveChanges();
                    return Ok("Book has been deleted successfully.");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to delete the book: {ex.Message}");
                }
            }catch(Exception ex)
            {
                return BadRequest($"Failed to delete the book: {ex.Message}");
            }
            
        }

        [Authorize]
        [HttpGet("byPublicationYear/{year}")]
        public IActionResult GetBooksByPublicationYear(int year)
        {
            try
            {
                var books = _context.Book.Where(b => b.PublicationYear == year).ToList();
                if (books != null)
                {
                    return Ok(books);
                }
                else
                {
                    return NotFound("No books found publicated in this year");
                }
            }catch (Exception ex)
            {
                return BadRequest($"Failed to get the books: {ex.Message}");
            }
            
        }

        [Authorize]
        [HttpGet("available")]
        public IActionResult GetAvailableBooks()
        {
            try
            {
                var availableBooks = _context.Book.Where(b => b.IsAvailable).ToList();
                if (availableBooks != null)
                {

                    return Ok(availableBooks);
                }
                else { return NotFound("there are no available books"); }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get the available books: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("byAuthor/{author}")]
        public IActionResult GetBooksByAuthor(string author)
        {
            try
            {
                var booksByAuthor = _context.Book.Where(b => b.Author == author).ToList();
                if (booksByAuthor != null)
                {
                    return Ok(booksByAuthor);
                }
                else { return NotFound("there are no books fot this author"); }
            }catch (Exception ex)
            {
                return BadRequest($"Failed to get author books: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("byTitle/{title}")]
        public IActionResult GetBookByTitle(string title)
        {
            try
            {
                var book = _context.Book.FirstOrDefault(b => b.Title == title);
                if (book == null)
                {
                    return NotFound("Book not found.");
                }
                return Ok(book);
            }catch(Exception ex)
            {
                return BadRequest($"Error {ex.Message}");
            }
            
        }

        [Authorize]
        [HttpGet("byBorrowingDate/{borrowDate}")]
        public IActionResult GetTransactionsByBorrowingDate(DateTime borrowDate)
        {
            try
            {
                var transactionsByBorrowDate = _context.BorrowingTransaction
                .Where(bt => bt.BorrowDate.Date == borrowDate.Date)
                .ToList();
                if (transactionsByBorrowDate != null)
                {
                    return Ok(transactionsByBorrowDate);
                }
                else { return NotFound("there are no books borrowed in this date"); }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get the date: {ex.Message}");
            }

        }

        [Authorize]
        [HttpGet("byReturnDate/{returnDate}")]
        public IActionResult GetTransactionsByReturnDate(DateTime returnDate)
        {
            try
            {
                var transactionsByReturnDate = _context.BorrowingTransaction
                .Where(bt => bt.ReturnDate.HasValue && bt.ReturnDate.Value.Date == returnDate.Date)
                .ToList();
                if (transactionsByReturnDate != null)
                {
                    return Ok(transactionsByReturnDate);
                }
                else { return NotFound("there are no books returned in this date"); }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get the date: {ex.Message}");
            }

        }

        [Authorize]
        [HttpGet("byBookId/{bookId}")]
        public IActionResult GetTransactionsByBook(int bookId)
        {
            try
            {
                var transactionsByBook = _context.BorrowingTransaction
                .Where(bt => bt.BookId == bookId)
                .ToList();
                if (transactionsByBook != null)
                {
                    return Ok(transactionsByBook);
                }
                else
                {
                    return NotFound("No books found with this ID");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get the book Id: {ex.Message}");
            }

        }

        [Authorize]
        [HttpGet("bookBorrowCount/{bookId}")]
        public IActionResult GetBookBorrowCount(int bookId)
        {
            try
            {
                var borrowCount = _context.BorrowingTransaction
                .Count(bt => bt.BookId == bookId);
                if (borrowCount != 0)
                {
                    return Ok(borrowCount);
                }
                else
                {
                    return NotFound("No history found for this book");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get the history of this book: {ex.Message}");
            }
        }


    }
}
