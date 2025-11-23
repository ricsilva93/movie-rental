using Microsoft.EntityFrameworkCore;
using MovieRental.Configuration.Exceptions;
using MovieRental.Controllers.Dtos;
using MovieRental.Controllers.DTOs;
using MovieRental.Data;
using MovieRental.PaymentProviders;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;

        private readonly IPaymentProviderResolver _paymentProviderFactory;
		public RentalFeatures(
            MovieRentalDbContext movieRentalDb,
            IPaymentProviderResolver paymentProviderFactory)
		{
			_movieRentalDb = movieRentalDb;
            _paymentProviderFactory = paymentProviderFactory;
		}

		//TODO: make me async :(
		public async Task<Rental> SaveAsync(Rental rental, CancellationToken cancellationToken = default)
		{
            var paymentProvider = _paymentProviderFactory.GetPaymentProviderByName(rental.PaymentMethod);

            var price = new Random().NextDouble() * rental.DaysRented;
            var paymentSucceeded = await paymentProvider.Pay(price);

            if (!paymentSucceeded)
            {
                throw new PaymentFailedException("Payment failed.");
            }

			_movieRentalDb.Rentals.Add(rental);
			await _movieRentalDb.SaveChangesAsync();
			
            return rental;
        }

		//TODO: finish this method and create an endpoint for it
		public async Task<PagedResult<RentalResponseDto>> GetRentalsByCustomerNameAsync(
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
                    rental.Customer!.Name.Contains(customerName));
            }

            var total = await query.CountAsync(cancellationToken);

            var result = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(rental => new RentalResponseDto(
                    rental.Id,
                    rental.CustomerId,
                    rental.MovieId,
                    rental.DaysRented,
                    rental.PaymentMethod,
                    rental.Movie!.Title,
                    rental.Customer!.Name
                ))
                .ToListAsync(cancellationToken);

            return new PagedResult<RentalResponseDto>(result, page, pageSize, total);
        }

	}
}
