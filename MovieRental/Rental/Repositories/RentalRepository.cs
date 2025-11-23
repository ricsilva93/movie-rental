using Microsoft.EntityFrameworkCore;
using MovieRental.Controllers.DTOs;
using MovieRental.Data;

namespace MovieRental.Rental.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly MovieRentalDbContext _movieRentalDb;

        public RentalRepository(MovieRentalDbContext movieRentalDb)
        {
            _movieRentalDb = movieRentalDb;
        }

        public async Task<PagedResult<RentalResponseDto>> GetRentalsByCustomerNameAsync(string customerName, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
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
                    rental.Customer!.Name,
                    rental.Movie!.Title
                ))
                .ToListAsync(cancellationToken);

            return new PagedResult<RentalResponseDto>(result, page, pageSize, total);
        }

        public async Task<Rental> SaveAsync(Rental rental, CancellationToken cancellationToken = default)
        {
            _movieRentalDb.Rentals.Add(rental);
            await _movieRentalDb.SaveChangesAsync();

            return rental;
        }
    }
}
