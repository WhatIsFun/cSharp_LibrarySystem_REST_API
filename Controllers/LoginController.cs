using cSharp_LibrarySystemWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cSharp_LibrarySystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public string Token { get; set; }
        public static LibraryDbContext _context;
        public LoginController(LibraryDbContext DB)
        {
            _context = DB;
        }

        [HttpPost("AdminLogin")]
        public IActionResult EmployeeLogin(EmployeeLogin login)
        {
            Log.Information("new request to login employee : " + login.Email);
            try
            {
                var userLogin = _context.Login.Where(n => n.Email == login.Email && n.Password == login.Password).FirstOrDefault();

                if (userLogin != null)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));

                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                    var data = new List<Claim>();
                    data.Add(new Claim("Name", userLogin.Name));

                    var token = new JwtSecurityToken(
                    issuer: "Mohammed",
                    audience: "TRA",
                    claims: data,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials

                    );
                    Log.Information($"new Login username: {userLogin.Name}, {login.Email}, {login.Password}");
                    string Token = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(Token);


                }
                else
                {
                    Log.Information("new unauthorized login employee : " + login.Email);
                    return Unauthorized("the user doesn't exist");
                }
            }
            catch (Exception ex)
            {
                Log.Error("new error to login employee : " + login.Email);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        public IActionResult UserLogin(EmployeeLogin login)
        {
            Log.Information("new request to login: " + login.Email);
            try
            {
                Patron user = _context.Patron.FirstOrDefault(u => u.Email == login.Email);
                if (user != null)
                {
                    if (VerifyPassword(login.Password, user.Password))
                    {
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));

                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                        var data = new List<Claim>();
                        data.Add(new Claim("Name", user.Name));

                        var token = new JwtSecurityToken(
                        issuer: "Mohammed",
                        audience: "TRA",
                        claims: data,
                        expires: DateTime.Now.AddMinutes(120),
                        signingCredentials: credentials

                        );
                        Log.Information($"new Login username: {user.Name}, {login.Email}, {login.Password}");
                        string Token = new JwtSecurityTokenHandler().WriteToken(token);
                        return Ok(Token);
                 //       return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    }
                    return Unauthorized("Invalid password.");

                }
                else
                {
                    Log.Information("new unauthorized login employee : " + login.Email);
                    return Unauthorized("the user doesn't exist");
                }
            }
            catch (Exception ex)
            {
                Log.Error("new error to login employee : " + login.Email);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet]
        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
