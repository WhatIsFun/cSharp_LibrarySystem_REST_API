using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using cSharp_LibrarySystemWebAPI.Model;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace cSharp_LibrarySystemWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        public static LibraryDbContext _context;

        public RegisterController(LibraryDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult Register(RegisterPatron User)
        {
            try
            {
                if (!IsValidEmail(User.Email))
                {
                    return Unauthorized("Invalid email address.");
                }

                if (!IsValidPassword(User.Password))
                {
                    return Unauthorized("Invalid password. Password must meet certain requirements.\nUppercase and Lowercase Letters\nDigits\nSpecial Characters (Minimum Length 8)");

                }
                string hashedPassword = HashPassword(User.Password); //hashing the password 

                // If email and password are valid, insert data into the database
                InsertUserRegistrationData(User.Name, User.Email, hashedPassword, User.PhoneNum, User.Age);


                return Ok("User registration successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        private static bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$"; //Regular expression for email validation
            return Regex.IsMatch(email, pattern);
        }

        private static bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"; //Uppercase and Lowercase Letters, Digits, and Special Characters (Minimum Length 8):

            Regex regex = new Regex(pattern);
            return regex.IsMatch(password); // Return true if password meets your requirements
        }

        // Insert user registration data into the database

        private static void InsertUserRegistrationData(string name, string email, string password, string phoneNum, int age)
        {
            try
            {
                var usr1 = new Patron {
                    Name = name, 
                    Email = email, 
                    Password = password,
                    PhoneNum = phoneNum,
                    Age = age 
                };
                _context.Add(usr1);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        private static string HashPassword(string password)
        {
            // BCrypt to hash the password
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
    
}
