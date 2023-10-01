using cSharp_LibrarySystemWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cSharp_LibrarySystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatronController : ControllerBase
    {
        public static LibraryDbContext _context;
        public PatronController(LibraryDbContext DB)
        {
            _context = DB;
        }

        [HttpPost]
        public void AddPatron(string name, string phone, int age)
        {
            var newPatron = new Patron
            {
                Name = name,
                PhoneNum = phone,
                Age = age
            };

            _context.Add(newPatron);
            _context.SaveChanges();

            Console.WriteLine("Patron added successfully. Press any key to continue.");
            Console.ReadKey();
        }
        [HttpGet]
        public List<Patron> GetAllPatrons()
        {
            return _context.Patron.ToList();
        }
        [HttpGet("getById")]
        public Patron GetPatronById(int patronId)
        {
            return _context.Patron.FirstOrDefault(p => p.PatronId == patronId);
        }
        [HttpGet("getByKeyword")]
        public List<Patron> SearchPatrons(string searchKeyword)
        {
            return _context.Patron
                .Where(p => p.Name.Contains(searchKeyword) || p.PhoneNum.Contains(searchKeyword))
                .ToList();
        }
        [HttpGet("byAgeRange/{minAge}/{maxAge}")]
        public void GetPatronsByAgeRange(int minAge, int maxAge)
        {
            var patronsInAgeRange = _context.Patron.Where(p => p.Age >= minAge && p.Age <= maxAge).ToList();
        }
        [HttpGet("byName/{name}")]
        public void GetPatronByName(string name)
        {
            var patron = _context.Patron.FirstOrDefault(p => p.Name == name);
        }
        [HttpPut]
        public void UpdatePatron(Patron patron)
        {
            var existingPatron = _context.Patron.FirstOrDefault(p => p.PatronId == patron.PatronId);

            if (existingPatron != null)
            {
                existingPatron.Name = patron.Name;
                existingPatron.PhoneNum = patron.PhoneNum;

                _context.SaveChanges();
            }
        }
        [HttpDelete]
        public void DeletePatron(int patronId)
        {
            var patronToDelete = _context.Patron.FirstOrDefault(p => p.PatronId == patronId);

            if (patronToDelete != null)
            {
                _context.Patron.Remove(patronToDelete);
                _context.SaveChanges();
            }
        }
    }
}
