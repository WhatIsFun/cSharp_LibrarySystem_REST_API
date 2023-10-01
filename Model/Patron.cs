using System.ComponentModel.DataAnnotations;

namespace cSharp_LibrarySystemWebAPI.Model
{
    public class Patron
    {
        [Key]
        public int PatronId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNum { get; set; }
        public int Age { get; set; }
        public List<BorrowingTransaction> BorrowingTransactions { get; set; }
    }
}
