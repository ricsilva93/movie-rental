using System.ComponentModel.DataAnnotations;

namespace MovieRental.Movie
{
	public class Movie
	{
		[Key]
		public int Id { get; set; }

        [MaxLength(70)]
        public required string Title { get; set; }

	}
}
