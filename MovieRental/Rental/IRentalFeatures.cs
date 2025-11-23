using MovieRental.Controllers.Dtos;
using MovieRental.Controllers.DTOs;

namespace MovieRental.Rental;

public interface IRentalFeatures
{
    Task<PagedResult<RentalResponseDto>> GetRentalsByCustomerNameAsync(
        string customerName,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<Rental> SaveAsync(
        Rental rental,
        CancellationToken cancellationToken = default);
}