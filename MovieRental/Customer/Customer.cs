using System.ComponentModel.DataAnnotations;

namespace MovieRental.Customer
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(70)]
        public required string Name { get; set; }
    }
}
