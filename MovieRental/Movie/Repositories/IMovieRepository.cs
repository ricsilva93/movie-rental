using MovieRental.Controllers.DTOs;

namespace MovieRental.Movie.Repositories
{
    public interface IMovieRepository
    {
        Task<Movie> SaveAsync(
                Movie movie,
                CancellationToken cancellationToken = default);

        Task<PagedResult<MovieResponseDto>> GetAllAsync(
            int page = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default);
    }
}
