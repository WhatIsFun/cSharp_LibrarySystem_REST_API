using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
//using Newtonsoft.Json;

namespace cSharp_LibrarySystemWebAPI.Model
{
    public class Patron
    {
        [Key]
        [JsonIgnore]
        public int PatronId { get; set; }

        [Required]
        
        public string Name { get; set; }
        [Required] 
        public string Email { get; set; }
        [Required] 
        public string Password { get; set; }
        [Required]
     
        public string PhoneNum { get; set; }
       
        public int Age { get; set; }

        [JsonIgnore]
        public List<BorrowingTransaction> BorrowingTransactions { get; set; }
    }
}
