using MovieRental.Controllers.Dtos;

namespace MovieRental.Movie;

public interface IMovieFeatures
{
    Movie SaveAsync(
            Movie movie,
            CancellationToken cancellationToken = default);

    Task<PagedResult<Movie>> GetAllAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);
}