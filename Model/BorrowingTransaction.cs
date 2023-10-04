using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cSharp_LibrarySystemWebAPI.Model
{
    public class BorrowingTransaction
    {
        [Key]
        [JsonIgnore]
        public int BorrowingTransactionId { get; set; }
        [ForeignKey("Patron")]
        public int PatronId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        [Required]
        [JsonIgnore]
        public DateTime BorrowDate { get; set; }
        [JsonIgnore]
        public DateTime? ReturnDate { get; set; }
        [JsonIgnore]
        public Patron Patron { get; set; }
        [JsonIgnore]
        public Book Book { get; set; }
    }
}
