using Microsoft.EntityFrameworkCore;
using MovieRental.Data;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		public RentalFeatures(MovieRentalDbContext movieRentalDb)
		{
			_movieRentalDb = movieRentalDb;
		}

		//TODO: make me async :(
		public async Task<Rental> Save(Rental rental)
		{
			await _movieRentalDb.Rentals.AddAsync(rental);
			var result = await _movieRentalDb.SaveChangesAsync();
			if (result > 0) return rental;
			else throw new Exception();
        }

		//TODO: finish this method and create an endpoint for it
		public IEnumerable<Rental> GetRentalsByCustomerName(string customerName)
		{
			return [];
		}

	}
}
