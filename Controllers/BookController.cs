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
    }
}
