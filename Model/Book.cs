using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cSharp_LibrarySystemWebAPI.Model
{
    public class Book
    {
        [Key]
        [JsonIgnore]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int PublicationYear { get; set; }
        [Required]
        [JsonIgnore]
        public bool IsAvailable { get; set; }
        [JsonIgnore]
        public List<BorrowingTransaction> BorrowingTransactions { get; set; }
    }
}
