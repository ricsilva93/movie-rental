using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRental.Rental
{
	public class Rental
	{
		[Key]
		public int Id { get; set; }

		public int DaysRented { get; set; }

        public int MovieId { get; set; }

        [ForeignKey(nameof(MovieId))]
        public Movie.Movie? Movie { get; set; }

		public string PaymentMethod { get; set; } = string.Empty;

        // TODO: we should have a table for the customers
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
		public Customer.Customer? Customer { get; set; }

	}
}
