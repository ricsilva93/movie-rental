using MovieRental.Configuration.Validation;
using MovieRental.Controllers.DTOs;
using MovieRental.Movie.Repositories;

namespace MovieRental.Movie
{
    public class MovieFeatures : IMovieFeatures
    {
        private readonly IMovieRepository _movieRepository;
        public MovieFeatures(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<Movie> SaveAsync(
            Movie movie,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(movie.Title))
            {
                throw new ArgumentException("Movie Title cannot be empty.");
            }

            return await _movieRepository.SaveAsync(movie, cancellationToken);
        }

        // TODO: tell us what is wrong in this method? Forget about the async, what other concerns do you have? Ans: please check README
        public async Task<PagedResult<MovieResponseDto>> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            (page, pageSize) = PaginationValidator.Normalize(page, pageSize);

            return await _movieRepository.GetAllAsync(page, pageSize, cancellationToken);
        }
    }
}
