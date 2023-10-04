using cSharp_LibrarySystemWebAPI.Model;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpPost("AddPatron")]
        public IActionResult AddPatron(string name, string phone, int age)
        {
            try
            {
                var newPatron = new Patron
                {
                    Name = name,
                    PhoneNum = phone,
                    Age = age
                };

                _context.Add(newPatron);
                _context.SaveChanges();

                return Ok("Patron added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("GetAllPatrons")]
        public IActionResult GetAllPatrons()
        {
            try
            {
                var patrons = _context.Patron.ToList();
                return Ok(patrons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("GetPatronById")]
        public IActionResult GetPatronById(int patronId)
        {
            try
            {
                var patron = _context.Patron.FirstOrDefault(p => p.PatronId == patronId);
                if (patron == null)
                {
                    return NotFound("Patron not found.");
                }
                return Ok(patron);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("SearchPatrons")]
        public IActionResult SearchPatrons(string searchKeyword)
        {
            try
            {
                var patrons = _context.Patron
                    .Where(p => p.Name.Contains(searchKeyword) || p.PhoneNum.Contains(searchKeyword))
                    .ToList();
                return Ok(patrons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("GetPatronsByAgeRange/{minAge}/{maxAge}")]
        public IActionResult GetPatronsByAgeRange(int minAge, int maxAge)
        {
            try
            {
                var patronsInAgeRange = _context.Patron.Where(p => p.Age >= minAge && p.Age <= maxAge).ToList();
                return Ok(patronsInAgeRange);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("GetPatronByName/{name}")]
        public IActionResult GetPatronByName(string name)
        {
            try
            {
                var patron = _context.Patron.FirstOrDefault(p => p.Name == name);
                if (patron == null)
                {
                    return NotFound("Patron not found.");
                }
                return Ok(patron);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPut("UpdatePatron")]
        public IActionResult UpdatePatron(Patron patron)
        {
            try
            {
                var existingPatron = _context.Patron.FirstOrDefault(p => p.PatronId == patron.PatronId);
                if (existingPatron != null)
                {
                    existingPatron.Name = patron.Name;
                    existingPatron.PhoneNum = patron.PhoneNum;

                    _context.SaveChanges();
                    return Ok("Patron updated successfully.");
                }
                else
                {
                    return NotFound("Patron not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpDelete("DeletePatron")]
        public IActionResult DeletePatron(int patronId)
        {
            try
            {
                var patronToDelete = _context.Patron.FirstOrDefault(p => p.PatronId == patronId);
                if (patronToDelete != null)
                {
                    _context.Patron.Remove(patronToDelete);
                    _context.SaveChanges();
                    return Ok("Patron deleted successfully.");
                }
                else
                {
                    return NotFound("Patron not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


    }
}
