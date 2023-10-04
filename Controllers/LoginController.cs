using cSharp_LibrarySystemWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cSharp_LibrarySystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public static LibraryDbContext _context;
        public LoginController(LibraryDbContext DB)
        {
            _context = DB;
        }
        [HttpPost("EmployeeLogin")]
        public IActionResult EmployeeLogin(string email, string password)
        {
            try
            {
                Login login = _context.Login.Where(n => n.Email == email && n.Password == password).FirstOrDefault();

                if (login != null)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));

                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                    var data = new List<Claim>();
                    data.Add(new Claim("Name", login.Name));

                    var token = new JwtSecurityToken(
                      issuer: "Mohammed",
                    audience: "TRA",
                    claims: data,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials

                    );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                }
                else
                {
                    return Unauthorized("the user doesn't exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
