using Microsoft.EntityFrameworkCore;
using MovieRental.Controllers.Dtos;
using MovieRental.Controllers.DTOs;
using MovieRental.Customer;
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
		public async Task<Rental> SaveAsync(Rental rental)
		{
			await _movieRentalDb.Rentals.AddAsync(rental);
			var result = await _movieRentalDb.SaveChangesAsync();
			if (result > 0) return rental;
			else throw new Exception();
        }

		//TODO: finish this method and create an endpoint for it
		public async Task<PagedResult<RentalDTO>> GetRentalsByCustomerNameAsync(
			string customerName,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
		{

            var query = _movieRentalDb.Rentals
                .AsNoTracking()
                .Include(rental => rental.Customer)
                .Include(rental => rental.Movie)
                .OrderBy(rental => rental.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                query = query.Where(rental =>
                    rental.Customer.Name.Contains(customerName)); //TODO me RECHECK
            }


            var total = await query.CountAsync(cancellationToken);

            var result = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(rental => new RentalDTO(
                    rental.Id,
                    rental.DaysRented,
                    rental.PaymentMethod
                    //rental.Customer,
                    //rental.Movie
                ))
                .ToListAsync(cancellationToken);

            return new PagedResult<RentalDTO>(result, page, pageSize, total);
        }

	}
}
