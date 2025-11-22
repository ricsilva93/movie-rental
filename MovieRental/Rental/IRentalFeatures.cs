using MovieRental.Controllers.Dtos;
using MovieRental.Controllers.DTOs;

namespace MovieRental.Rental;

public interface IRentalFeatures
{
    Task<Rental> SaveAsync(Rental rental);

    Task<PagedResult<RentalDTO>> GetRentalsByCustomerNameAsync(
        string customerName,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}