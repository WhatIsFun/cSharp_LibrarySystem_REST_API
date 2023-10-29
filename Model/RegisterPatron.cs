using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace cSharp_LibrarySystemWebAPI.Model
{
    public class RegisterPatron
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNum { get; set; }
        public int Age { get; set; }
    }
}
