using cSharp_LibrarySystemWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.WSIdentity;
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
        [HttpPost("EmployeeLogin")]
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
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));


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
    }
}
