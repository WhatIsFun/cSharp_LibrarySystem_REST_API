using System.ComponentModel.DataAnnotations;

namespace cSharp_LibrarySystemWebAPI.Model
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int PublicationYear { get; set; }
        [Required]
        public bool IsAvailable { get; set; }

        public List<BorrowingTransaction> BorrowingTransactions { get; set; }
    }
}
