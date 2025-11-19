using MovieRental.Controllers.Dtos;

namespace MovieRental.Movie;

public interface IMovieFeatures
{
	Movie Save(Movie movie);

    Task<PagedResult<Movie>> GetAll();
}