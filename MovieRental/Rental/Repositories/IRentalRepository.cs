using MovieRental.Controllers.DTOs;

namespace MovieRental.Rental.Repositories
{
    public interface IRentalRepository
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
}
